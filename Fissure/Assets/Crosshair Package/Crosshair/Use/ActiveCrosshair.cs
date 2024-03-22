using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Threading;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC, CLEAN SHIRT LABS
    /// Created: 10/30/2023
    /// Last Edited: 12/8/2023
    /// 
    /// <summary>
    /// A Handler for CrosshairData on the Canvas
    /// </summary>
    public class Crosshair : MonoBehaviour
    {
        [SerializeField, SerializeReference] DEMO_CrosshairUpdater m_updater;

        CrosshairData m_data;
        //Components to update OnCrosshair
        public RawImage c_dot;  //put the dot image here
        public RawImage c_inner;  //put the inner crosshair/ring here
        public RawImage c_expanding; //the image that is expanded when the gun shoots
        public Image c_reload; // the image for the reload meter
        public RawImage c_scope;// the image that cuts off view when in the scope

        //the starting scale
        Vector3 m_defaultScale;

        //Status
        bool m_isReloading; //used to determine whether we are already reloading (so that we have to finish reloading before we reload again).
        bool m_isShrinking; //used to make sure crosshair returns to normal size at the right speed;
        bool m_isScoped = false;
        Target m_target = Target.Standard;
        Display m_currentDisplay;

        //Reload Task
        CancellationTokenSource m_cancellationTokenSource = null;
        System.Threading.Tasks.Task m_reloadTask;

        /// <summary>
        /// Attach the starting gun. Should be deleted when WeaponManager is built
        /// </summary>
        void Awake()
        {
            m_updater.Expand += ExpandCrosshair;
            m_updater.Reload += DoReload;
            m_updater.Swap += SetCrosshair;
            m_updater.ToggleScope += ToggleScope;
            m_updater.UpdateTarget += (target) => { m_target = target; Display(); };
        }

        /// <summary>
        /// Switch the Crosshair view and respones to correspond with passed weapon
        /// </summary>
        /// <param name="data">Crosshair to reflect</param>
        private void SetCrosshair(CrosshairData data)
        {
            m_data = data;
            Display();
            EndReload();
            ///reset expanding
            m_defaultScale = c_expanding.rectTransform.localScale;
        }
        /// <summary>
        /// Handle the toggling scope event
        /// </summary>
        private void ToggleScope()
        {
            m_isScoped = !m_isScoped;
            Display();
        }
        /// <summary>
        /// Display the crosshair defined by the passed details
        /// </summary>
        /// <param name="display">Crosshair details to present</param>
        private void Display()
        {
            if (m_isScoped)
            {
                m_currentDisplay = m_data.Scope[m_target];
                c_scope.texture = m_data.Scope[m_target].Scope.Texture;
                c_scope.color = m_data.Scope[m_target].Scope.Color;
                c_scope.enabled = true;
            }
            else
            {
                c_scope.enabled = false;
                m_currentDisplay = m_data.Hip[m_target];
                c_reload.sprite = Sprite.Create(m_data.Hip[m_target].Reload.Texture, c_reload.sprite.rect, c_reload.sprite.pivot);
                c_reload.color = m_data.Hip[m_target].Reload.Color;
            }
            

            c_dot.texture = m_currentDisplay.Dot.Texture;
            c_inner.texture = m_currentDisplay.Inner.Texture;
            c_expanding.texture = m_currentDisplay.Expanding.Texture;
            
            //change color
            c_dot.color = m_currentDisplay.Dot.Color;
            c_inner.color = m_currentDisplay.Inner.Color;
            c_expanding.color = m_currentDisplay.Expanding.Color;
        }

        /// <summary>
        /// Handle Reload Crosshair Animation by limiting reload input
        /// </summary>
        private void DoReload(float reloadTime)
        {
            //if we are not already reloading, do the reloading routine.
            if (!m_isReloading)
                StartCoroutine(ReloadTheGun(reloadTime));
        }

        /// <summary>
        /// Expand Crosshair
        /// </summary>
        /// <param name="addScale">Amount to expand by</param>
        private void ExpandCrosshair(float addScale)
        {
            //if the crosshair is still under the maximum expandable size, then make it expand more
            if (c_expanding.rectTransform.localScale.x < m_currentDisplay.MaxScale)
            {
                c_expanding.rectTransform.localScale += new Vector3(addScale, addScale, addScale);
            }
            else
                c_expanding.rectTransform.localScale = new Vector3(m_currentDisplay.MaxScale, m_currentDisplay.MaxScale, m_currentDisplay.MaxScale);


            if (!m_isShrinking)  // slowly shrink the crosshair back to normal, if it's not already started shrinking
                StartCoroutine(ShrinkCrosshair());
        }

        /// <summary>
        /// Shrink Crosshair progressively until it is the default size
        /// </summary>
        /// <returns></returns>
        IEnumerator ShrinkCrosshair()
        {
            m_isShrinking = true; //make sure we don't shrink while shrinking


            //while the crosshair is bigger than default size, keep shrinking
            do
            {
                c_expanding.rectTransform.localScale = new Vector3(c_expanding.rectTransform.localScale.x - Time.deltaTime * m_currentDisplay.ShrinkSpeed,
                                                                 c_expanding.rectTransform.localScale.y - Time.deltaTime * m_currentDisplay.ShrinkSpeed,
                                                                 c_expanding.rectTransform.localScale.z - Time.deltaTime * m_currentDisplay.ShrinkSpeed);
                yield return new WaitForEndOfFrame();
            }
            while (m_defaultScale.x < c_expanding.rectTransform.localScale.x);

            m_isShrinking = false;
            yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// End Reload Animations
        /// </summary>
        private void EndReload()
        {
            if (m_isReloading)
            {
                m_cancellationTokenSource.Cancel();
            }
            c_reload.enabled = false;
        }
        /// <summary>
        /// Reload Animation
        /// </summary>
        /// <returns></returns>
        IEnumerator ReloadTheGun(float reloadTime)
        {
            //make sure our script knows we are now reloading, so we can't reload while already reloading
            m_isReloading = true;

            // reset the reload meter to 0, show it on the screen, and hide the regular crosshairs
            c_reload.fillAmount = 0;
            c_reload.enabled = true;
            c_inner.enabled = false;
            c_dot.enabled = false;
            c_expanding.enabled = false;

            float reloadTimer = 0f;


            m_cancellationTokenSource = new CancellationTokenSource();

            m_reloadTask = System.Threading.Tasks.Task.Delay((int)(reloadTime * 1000f));
            //if the  reload meter isn't filled/finished, then keep filling until it is or is cancelled
            do
            {
                c_reload.fillAmount = Mathf.Lerp(0, 1, reloadTimer);
                reloadTimer += Time.deltaTime / reloadTime;
                yield return new WaitForEndOfFrame();
                if (m_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    m_cancellationTokenSource.Dispose();
                    break;
                }
            }
            while (!m_reloadTask.IsCompleted);

            //hide the reload meter now that we are done reloading, and enable the regular crosshairs
            c_reload.enabled = false;
            c_inner.enabled = true;
            c_dot.enabled = true;
            c_expanding.enabled = true;


            // we're done reloading, setting this back to false, so we can reload next time
            m_isReloading = false;

            c_expanding.rectTransform.localScale = m_defaultScale; //return the expanding element to the default size
            
            yield return new WaitForEndOfFrame();
        }

    }
}
