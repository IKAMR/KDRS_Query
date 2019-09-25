using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace KDRS_Query
{
    class XPathQueryRunner
    {
        public void RunXPath(List<QueryClass> Xqueries, string xmlFileName)
        {

            XPathNavigator nav;
            XPathDocument docNav;
            XPathNodeIterator nodes;

            List<string> results = new List<string>();

            docNav = new XPathDocument(xmlFileName);

            nav = docNav.CreateNavigator();

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nav.NameTable);
            // var nameSpace = nav.GetNamespace(nav.SelectSingleNode("arkiv").NamespaceURI);
            //nsmgr.AddNamespace("a", nameSpace);

            nsmgr.AddNamespace("a", "http://www.arkivverket.no/standarder/noark5/arkivstruktur");
            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // C1.  Antall arkiver i arkivstrukturen
            results.Add("C1");
            XPathExpression query = nav.Compile("count(/a:arkiv)");
            query.SetContext(nsmgr);

            results.Add("Antall arkiv: " + nav.Evaluate(query));
            results.Add("-------------------------------------------------");
            //-------------------------------------------------


            // C2. Antall arkivdeler i arkivstrukturen
            results.Add("C2");
            XPathExpression query2 = nav.Compile("count(//a:arkivdel)");
            query2.SetContext(nsmgr);

            int antallArkivdeler = int.Parse(nav.Evaluate(query2).ToString());
            results.Add("Antall arkivdeler i arkiv '" + nav.SelectSingleNode("//a:arkiv/a:tittel", nsmgr) + "': " + nav.Evaluate(query2));
            results.Add("");

            nodes = nav.Select("//a:arkivdel", nsmgr);
            XPathExpression arkivdelTittel = nav.Compile("concat('\"', descendant::a:tittel, ' (', descendant::a:systemID, ')', '\"' )");
            arkivdelTittel.SetContext(nsmgr);

            while (nodes.MoveNext())
                results.Add(nav.Evaluate(arkivdelTittel, nodes).ToString());
            results.Add("-------------------------------------------------");
            //-------------------------------------------------

            foreach (XML_Query q in Xqueries)
            {
                Console.WriteLine("Query: " + q.JobName);
                nodes = nav.Select("//a:arkivdel", nsmgr);
                XPathExpression xPathEx = nav.Compile(q.Query);
                xPathEx.SetContext(nsmgr);

                while (nodes.MoveNext())
                    q.Result = nav.Evaluate(xPathEx, nodes).ToString().Replace("\\r\\n", "\r\n");
            }
         

        }
    }
}
