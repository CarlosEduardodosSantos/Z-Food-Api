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
//   limitations under the License.

using System.Collections.Generic;

namespace Uol.PagSeguro.Domain
{
    /// <summary>
    /// Represents a parameter
    /// </summary>
    public class Parameter
    {
        private IList<ParameterItem> _items;

        /// <summary>
        ///  List of available parameter item for send in checkout
        /// </summary>
        public IList<ParameterItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<ParameterItem>();
                }
                return _items;
            }
        }
    }
}