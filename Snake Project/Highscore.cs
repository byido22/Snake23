using System;
using System.IO;
using System.Xml.Serialization;

namespace Snake_Project
{
    [Serializable]
    public class Highscore
    {
        public int Value;

        public void Save(string FileName)
        {
            using (var stream = new FileStream(FileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(Highscore));
                XML.Serialize(stream, this);
            }
        }

        public static Highscore LoadFile(string FileName)
        {
            using (var stream = new FileStream(FileName, FileMode.Open))
            {
                var XML = new XmlSerializer(typeof(Highscore));
                return (Highscore)XML.Deserialize(stream);
            }
        }

    }
}