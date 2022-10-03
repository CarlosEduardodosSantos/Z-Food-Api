﻿// Copyright [2011] [PagSeguro Internet Ltda.]
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
using Uol.PagSeguro.Domain;

namespace Uol.PagSeguro.XmlParse
{
    /// <summary>
    ///
    /// </summary>
    internal static class PhoneSerializer
    {
        internal const string Phone = "phone";

        private const string AreaCode = "areaCode";
        private const string Number = "number";

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="phone"></param>
        internal static void Read(XmlReader reader, Phone phone)
        {
            if (reader.IsEmptyElement)
            {
                XMLParserUtils.SkipNode(reader);
                return;
            }

            reader.ReadStartElement(PhoneSerializer.Phone);
            reader.MoveToContent();

            while (!reader.EOF)
            {
                if (XMLParserUtils.IsEndElement(reader, PhoneSerializer.Phone))
                {
                    XMLParserUtils.SkipNode(reader);
                    break;
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case PhoneSerializer.AreaCode:
                            phone.AreaCode = reader.ReadElementContentAsString();
                            break;

                        case PhoneSerializer.Number:
                            phone.Number = reader.ReadElementContentAsString();
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