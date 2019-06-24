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
using System.Collections.Generic;

namespace Relinq {

    public struct ReplaceEnumerator<TEnumerator, TSource> : 
        IAdaptableEnumerator<TSource> 
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current => 
            m_equalityComparer.Equals(x:m_enumerator.Current, y:m_replaceWhat) ? 
            m_replaceWith : 
            m_enumerator.Current
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly TSource m_replaceWhat;
        private readonly TSource m_replaceWith;
        private readonly IEqualityComparer<TSource> m_equalityComparer;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in TEnumerator enumerator, 
            TSource what, 
            TSource with, 
            IEqualityComparer<TSource> equalityComparer
        ) =>
            new EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new ReplaceEnumerator<TEnumerator, TSource>(
                    enumerator:enumerator, 
                    replaceWhat:what, 
                    replaceWith:with, 
                    equalityComparer:equalityComparer
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private ReplaceEnumerator (
            in TEnumerator enumerator,
            TSource replaceWhat,
            TSource replaceWith,
            IEqualityComparer<TSource> equalityComparer
        ) {
            m_enumerator = enumerator;
            m_replaceWhat = replaceWhat;
            m_replaceWith = replaceWith;
            m_equalityComparer = 
                equalityComparer ?? throw new ArgumentNullException(paramName:nameof(equalityComparer))
            ;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_enumerator.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator.Reset();
    }
    
}