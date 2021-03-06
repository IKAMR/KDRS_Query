﻿using Saxon.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace KDRS_Query
{
    class XPathQueryRunner
    {
        public delegate void ProgressUpdate(string statusMsg);
        public event ProgressUpdate OnProgressUpdate;

        public void RunXPath(List<QueryClass> Xqueries, string sourceFolder)
        {
            Dictionary<string, XmlDocument> sources = new Dictionary<string, XmlDocument>();
            foreach (XML_Query q in Xqueries)
            {
                if (q.JobEnabled.Equals("1") || q.JobEnabled.Equals("2") || q.JobEnabled.Equals("3"))
                {

                    string xmlFileName = Path.Combine(sourceFolder, q.Source);
                    Console.WriteLine("Source: " + q.Source + ", XML File: " + xmlFileName);

                    Processor processor = new Processor();

                    XmlDocument inputDoc = new XmlDocument();

                   if (!sources.ContainsKey(q.Source))
                    {
                        XmlDocument newDoc = new XmlDocument();
                        newDoc.Load(xmlFileName);
                        sources.Add(q.Source, newDoc);
                    } 
                    inputDoc = sources[q.Source];

                    XdmNode xmlDoc = processor.NewDocumentBuilder().Build(new XmlNodeReader(inputDoc));

                    XPathCompiler xPathCompiler = processor.NewXPathCompiler();

                    string nameSpace = inputDoc.DocumentElement.NamespaceURI;
                    string nameSpaceXsi = inputDoc.DocumentElement.GetNamespaceOfPrefix("xsi");

                    xPathCompiler.DeclareNamespace("", nameSpace);
                    xPathCompiler.DeclareNamespace("xsi", nameSpaceXsi);

                    string query = q.Query;

                    try
                    {
                        OnProgressUpdate?.Invoke(q.JobId);
                        string result = xPathCompiler.Evaluate(query, xmlDoc).ToString();
                        
                        q.Result = result.Replace("\"", "");
                    }
                    catch (Exception e)
                    {
                        OnProgressUpdate?.Invoke("ERROR: " + q.JobId);
                        q.Result = "ERROR 1, unable to compile: " + q.Query;
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        //NOT IN USE!
        // Runs XPath from Xqueries on file spesified in query. Either 'arkivstruktur.xml, loependeJournal.xml, offentligJournal.xml or endringslogg.xml.
        public void RunXpath2(List<QueryClass> Xqueries, string sourceFolder)
        {
            foreach (XML_Query q in Xqueries)
            {
                if (q.JobEnabled.Equals("1") || q.JobEnabled.Equals("2"))
                {
                    XPathNavigator nav;
                    XPathDocument docNav;
                    XPathNodeIterator nodes;
                    XPathExpression xPathEx;

                    string queryNodes = String.Empty;
                    bool splitResults = true;

                    string xmlFileName = Path.Combine(sourceFolder, q.Source);
                    Console.WriteLine("Source: " + q.Source + ", XML File: " + xmlFileName);

                    docNav = new XPathDocument(xmlFileName);

                    nav = docNav.CreateNavigator();

                    string queryText = q.Query;
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(nav.NameTable);
                    if (q.Source.Equals("arkivstruktur.xml"))
                    {
                        nsmgr.AddNamespace("a", "http://www.arkivverket.no/standarder/noark5/arkivstruktur");
                        nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                        string splitAtConcat = "concat";

                        Console.WriteLine("Query id: " + q.JobId);
                        Console.WriteLine("Query: " + q.Query);
                        Console.WriteLine("Index of concat: " + q.Query.IndexOf(splitAtConcat));

                        if (q.Query.Substring(0, 4).Contains(@"//a:") && q.Query.Contains(splitAtConcat) && q.Query.IndexOf(splitAtConcat) > 0)
                        {
                            queryNodes = q.Query.Substring(0, q.Query.IndexOf(splitAtConcat) - 1);
                            queryText = q.Query.Substring(q.Query.IndexOf(splitAtConcat));

                            splitResults = false;
                        }

                        Console.WriteLine("queryNodes: " + queryNodes);
                        Console.WriteLine("queryText: " + queryText);


                        nodes = nav.Select("//a:arkivdel" + queryNodes, nsmgr);
                        try
                        {
                            xPathEx = nav.Compile(queryText);
                            xPathEx.SetContext(nsmgr);

                            while (nodes.MoveNext())
                            {
                                string result = nav.Evaluate(xPathEx, nodes).ToString().Replace("\\r\\n", "\r\n");
                                if (q.Result != null && !q.Result.Equals(result))
                                {
                                    if (splitResults)
                                        q.Result += "\r\n\r\n" + result;
                                    else
                                        q.Result += "\r\n" + result;
                                }
                                else
                                    q.Result = result;
                            }
                        }
                        catch (Exception e)
                        {
                            q.Result = "ERROR 1, unable to compile: " + q.Query;
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        nsmgr.AddNamespace("l", "http://www.arkivverket.no/standarder/noark5/loependeJournal");
                        nsmgr.AddNamespace("o", "http://www.arkivverket.no/standarder/noark5/offentligJournal");
                        nsmgr.AddNamespace("e", "http://www.arkivverket.no/standarder/noark5/endringslogg");
                        try
                        {
                            xPathEx = nav.Compile(queryText);
                            xPathEx.SetContext(nsmgr);

                            q.Result = nav.Evaluate(xPathEx).ToString().Replace("\\r\\n", "\r\n");
                        }catch (Exception e)
                        {
                            q.Result = "ERROR 2, unable to compile: " + q.Query;
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}
