using System;
using System.Collections.Generic;
using System.IO;
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
            foreach (XML_Query q in Xqueries)
            {
                Console.WriteLine("Query id: " + q.JobId);
                Console.WriteLine("Query: " + q.Query);

                nodes = nav.Select("//a:arkivdel", nsmgr);
                XPathExpression xPathEx = nav.Compile(q.Query);
                xPathEx.SetContext(nsmgr);

                while (nodes.MoveNext())
                    q.Result = nav.Evaluate(xPathEx, nodes).ToString().Replace("\\r\\n", "\r\n");
            }
        }

        public void RunXpath2(List<QueryClass> Xqueries, string sourceFolder)
        {
            foreach (XML_Query q in Xqueries)
            {
                if (q.JobEnabled.Equals("1"))
                {
                    XPathNavigator nav;
                    XPathDocument docNav;
                    XPathNodeIterator nodes;

                    List<string> results = new List<string>();

                    string xmlFileName = Path.Combine(sourceFolder, q.Source);
                    Console.WriteLine("Source: " + q.Source + ", XML File: " + xmlFileName);

                    docNav = new XPathDocument(xmlFileName);

                    nav = docNav.CreateNavigator();

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(nav.NameTable);
                    // var nameSpace = nav.GetNamespace(nav.SelectSingleNode("arkiv").NamespaceURI);
                    //nsmgr.AddNamespace("a", nameSpace);
                    if (q.Source.Equals("arkivstruktur.xml"))
                    {
                        nsmgr.AddNamespace("a", "http://www.arkivverket.no/standarder/noark5/arkivstruktur");
                        nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                        Console.WriteLine("Query id: " + q.JobId);
                        Console.WriteLine("Query: " + q.Query);

                        nodes = nav.Select("//a:arkivdel", nsmgr);
                        XPathExpression xPathEx = nav.Compile(q.Query);
                        xPathEx.SetContext(nsmgr);

                        while (nodes.MoveNext())
                            q.Result += nav.Evaluate(xPathEx, nodes).ToString().Replace("\\r\\n", "\r\n") + "\r\n\r\n";
                    }
                    else
                    {
                        nsmgr.AddNamespace("l", "http://www.arkivverket.no/standarder/noark5/loependeJournal");
                        nsmgr.AddNamespace("o", "http://www.arkivverket.no/standarder/noark5/offentligJournal");
                        nsmgr.AddNamespace("e", "http://www.arkivverket.no/standarder/noark5/endringslogg");

                        XPathExpression xPathEx = nav.Compile(q.Query);
                        xPathEx.SetContext(nsmgr);
                        q.Result = nav.Evaluate(xPathEx).ToString().Replace("\\r\\n", "\r\n");
                    }
                }
            }
        }
    }
}
