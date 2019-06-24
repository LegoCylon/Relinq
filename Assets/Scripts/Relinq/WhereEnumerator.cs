//======================================================================================================================
//  Copyright 2019 Andy Bond (LegoCylon)
//  
//  Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
//  the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
//  an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and limitations under the License.
//======================================================================================================================

using System;

namespace Relinq {

    public struct WhereEnumerator<TEnumerator, TSource> : 
        IAdaptableEnumerator<TSource>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current => m_enumerator.Current;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly Func<TSource, bool> m_predicate;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<WhereEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in TEnumerator enumerator,
            Func<TSource, bool> predicate
        ) =>
            new EnumerableAdapter<WhereEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new WhereEnumerator<TEnumerator, TSource>(enumerator:enumerator, predicate:predicate)
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private WhereEnumerator (in TEnumerator enumerator, Func<TSource, bool> predicate) {
            m_enumerator = enumerator;
            m_predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            while (m_enumerator.MoveNext()) {
                if (m_predicate(arg:m_enumerator.Current)) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator.Reset();
    }
    
}