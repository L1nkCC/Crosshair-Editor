namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 12/5/2023
    /// Last Edited: 12/5/2023
    /// 
    /// <summary>
    /// Describer for what is in crosshair's view
    /// </summary>
    public enum Target
    {
        Standard,
        Enemy,
        Friendly,
    }

    [System.Serializable]
    public class DisplayGroup<T> where T : Display
    {
        [UnityEngine.SerializeField]
        private T[] m_displays = new T[System.Enum.GetValues(typeof(Target)).Length];
        public T this[Target target]
        {
            get { return m_displays[(int)target]; }
            set { m_displays[(int)target] = value; }
        }
        public System.Type Type => typeof(T);
    }
}
