namespace adif
{
    /// <summary>
    /// This holds a key and field data.
    /// </summary>
    public class LogFieldElement
    {
        public string ADIFTag;
        public string Data;
        public LogFieldElement(string k)
        {
            ADIFTag = k;
            Data = "";
        }
        public LogFieldElement(string k, string d)
        {
            ADIFTag = k;
            Data = d;
        }
        public LogFieldElement(LogFieldElement e)
        {
            ADIFTag = e.ADIFTag;
            Data = e.Data;
        }
    }
}
