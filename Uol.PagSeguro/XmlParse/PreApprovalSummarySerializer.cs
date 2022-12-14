// Copyright [2011] [PagSeguro Internet Ltda.]
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitation

using System.Xml;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;

namespace Uol.PagSeguro.XmlParse
{
    /// <summary>
    ///
    /// </summary>
    internal static class PreApprovalSummarySerializer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="preApproval"></param>
        internal static void Read(XmlReader reader, PreApprovalSummary preApproval)
        {
            if (reader.IsEmptyElement)
            {
                XMLParserUtils.SkipNode(reader);
                return;
            }

            reader.ReadStartElement(SerializerHelper.PreApproval);

            reader.MoveToContent();

            while (!reader.EOF)
            {
                if (XMLParserUtils.IsEndElement(reader, SerializerHelper.PreApproval))
                {
                    XMLParserUtils.SkipNode(reader);
                    break;
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case SerializerHelper.Code:
                            preApproval.Code = reader.ReadElementContentAsString();
                            break;

                        case SerializerHelper.Date:
                            preApproval.Date = reader.ReadElementContentAsDateTime();
                            break;

                        case SerializerHelper.Reference:
                            preApproval.Reference = reader.ReadElementContentAsString();
                            break;

                        case SerializerHelper.TransactionStatus:
                            preApproval.Status = reader.ReadElementContentAsString();
                            break;

                        case SerializerHelper.LastEventDate:
                            preApproval.LastEventDate = reader.ReadElementContentAsDateTime();
                            break;

                        case SerializerHelper.Name:
                            preApproval.Name = reader.ReadElementContentAsString();
                            break;

                        case SerializerHelper.Tracker:
                            preApproval.Tracker = reader.ReadElementContentAsString();
                            break;

                        case SerializerHelper.Charge:
                            preApproval.Charge = reader.ReadElementContentAsString();
                            break;

                        default:
                            XMLParserUtils.SkipElement(reader);
                            break;
                    }
                }
                else
                {
                    XMLParserUtils.SkipNode(reader);
                }
            }
        }
    }
}