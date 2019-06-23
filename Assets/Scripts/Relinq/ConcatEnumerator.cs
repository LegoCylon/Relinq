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

    public struct ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource> : 
        IAdaptedEnumerator<ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>, TSource>
        where TFirstEnumerator : IAdaptedEnumerator<TFirstEnumerator, TSource>
        where TSecondEnumerator : IAdaptedEnumerator<TSecondEnumerator, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Types
        //--------------------------------------------------------------------------------------------------------------
        private enum State {
            Default,
            UsingFirst,
            UsingSecond,
            Finished,
        }
        
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>, TSource> 
            Description { get;} = 
            new EnumeratorDescription<ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>, TSource>(
                current:(ref ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource> enumerator) => 
                    enumerator.Current,
                dispose:(ref ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource> enumerator) => 
                    enumerator.Dispose(),
                moveNext:(ref ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource> enumerator) => 
                    enumerator.MoveNext(),
                reset:(ref ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource> enumerator) => 
                    enumerator.Reset() 
            )
        ;
        private TSource Current {
            get {
                switch (m_state) {
                    case State.UsingFirst:
                        return m_first.Current;
                    case State.UsingSecond:
                        return m_second.Current;
                    case State.Default:
                    case State.Finished:
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TFirstEnumerator, TSource> m_first;
        private EnumeratorAdapter<TSecondEnumerator, TSource> m_second;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>, TSource>
            GetEnumerable (
                in EnumeratorAdapter<TFirstEnumerator, TSource> first,
                in EnumeratorAdapter<TSecondEnumerator, TSource> second
            ) =>
            new EnumerableAdapter<ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<
                    ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>, 
                    TSource
                >(
                    description:Description,
                    enumerator:new ConcatEnumerable<TFirstEnumerator, TSecondEnumerator, TSource>(
                        first:first,
                        second:second
                    )
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private ConcatEnumerable (
            in EnumeratorAdapter<TFirstEnumerator, TSource> first, 
            in EnumeratorAdapter<TSecondEnumerator, TSource> second
        ) {
            m_first = first;
            m_second = second;
            m_state = State.Default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () {
            m_first.Dispose();
            m_second.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            for (;;) {
                switch (m_state) {
                    case State.Default:
                        m_state = State.UsingFirst;
                        break;
                    case State.UsingFirst:
                        if (m_first.MoveNext()) {
                            return true;
                        }
                        m_state = State.UsingSecond;
                        break;
                    case State.UsingSecond:
                        if (m_second.MoveNext()) {
                            return true;
                        }
                        m_state = State.Finished;
                        break;
                    case State.Finished:
                        return false;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_first.Reset();
            m_second.Reset();
            m_state = State.Default;
        }
    }
    
}