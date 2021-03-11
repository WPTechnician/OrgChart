using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PopOrgChart
{
    class JSONGen
    {
        public List<ADUser> FlatToHierarchy(List<ADUser> list)
        {
            foreach (var User in list)
            {
                     User.children = (from children in list where User.FullName == children.Manager select children).ToList();
            }
            //Change CEO Name to the Person who would be at the top of the hierarchy
            list.RemoveAll(x => x.FullName != "CEO NAME");
            return list;
        }
        public List<ADUser> RemoveNulls(List<ADUser> list)
        {
            foreach (var User in list)
            {
                //Change CEO Name to the Person who would be at the top of the hierarchy
                if (User.Manager.Length == 0 && User.FullName != "CEO Name")
                {
                    User.empty = true;
                }
            }
            list.RemoveAll(x => x.empty == true);
            return list;
        }

        public string ConvertoJSON<ADUser>(List<ADUser> ADUsers)
        {
            // Json.NET serializer.
            JsonSerializer serializer = new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serialize the main employee list.
            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw)
            {
                Formatting = Formatting.Indented,
                QuoteChar = '"',
                QuoteName = true
            };

            serializer.Serialize(writer, ADUsers);
            string jsonStr = sw.ToString();
            return jsonStr;
        }
    }
}