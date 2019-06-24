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

    public struct ZipEnumerator<TFirstEnumerator, TFirstSource, TSecondEnumerator, TSecondSource, TResult> : 
        IAdaptableEnumerator<TResult>
        where TFirstEnumerator : IAdaptableEnumerator<TFirstSource>
        where TSecondEnumerator : IAdaptableEnumerator<TSecondSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TResult Current => m_resultSelector(arg1:m_first.Current, arg2:m_second.Current);

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TFirstEnumerator m_first;
        private TSecondEnumerator m_second;
        private readonly Func<TFirstSource, TSecondSource, TResult> m_resultSelector;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<
            ZipEnumerator<TFirstEnumerator, TFirstSource, TSecondEnumerator, TSecondSource, TResult>,
            TResult
        >
            GetEnumerable (
                in TFirstEnumerator first,
                in TSecondEnumerator second,
                Func<TFirstSource, TSecondSource, TResult> resultSelector
            ) =>
            new EnumerableAdapter<
                ZipEnumerator<TFirstEnumerator, TFirstSource, TSecondEnumerator, TSecondSource, TResult>, 
                TResult
            >(
                enumerator:new ZipEnumerator<
                    TFirstEnumerator, 
                    TFirstSource, 
                    TSecondEnumerator, 
                    TSecondSource, 
                    TResult
                >(first:first, second:second, resultSelector:resultSelector)
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private ZipEnumerator (
            in TFirstEnumerator first, 
            in TSecondEnumerator second, 
            Func<TFirstSource, TSecondSource, TResult> resultSelector
        ) {
            m_first = first;
            m_second = second;
            m_resultSelector = resultSelector ?? throw new ArgumentNullException(paramName:nameof(resultSelector));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () {
            m_first.Dispose();
            m_second.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_first.MoveNext() && m_second.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_first.Reset();
            m_second.Reset();
        }
    }
    
}