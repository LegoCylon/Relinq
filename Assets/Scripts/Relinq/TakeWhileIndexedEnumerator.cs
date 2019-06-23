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

    public struct TakeWhileIndexedEnumerator<TEnumerator, TSource> : 
        IAdaptableEnumerator<TSource>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Types
        //--------------------------------------------------------------------------------------------------------------
        private enum State {
            Default,
            Enumerating,
            Finished,
        }
        
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource> 
            Description { get; } = 
            new EnumeratorDescription<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref TakeWhileIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref TakeWhileIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref TakeWhileIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref TakeWhileIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current {
            get {
                switch (m_state) {
                    case State.Enumerating:
                        return m_enumerator.Current;
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
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<TSource, int, bool> m_predicate;
        private State m_state;
        private int m_taken;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, int, bool> predicate
        ) =>
            new EnumerableAdapter<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new TakeWhileIndexedEnumerator<TEnumerator, TSource>(
                        enumerator:enumerator, 
                        predicate:predicate
                    ) 
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private TakeWhileIndexedEnumerator (
            EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            Func<TSource, int, bool> predicate
        ) {
            m_enumerator = enumerator;
            m_predicate = predicate;
            m_state = State.Default;
            m_taken = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            for (;;) {
                switch (m_state) {
                    case State.Default:
                        m_state = State.Enumerating;
                        break;
                    case State.Enumerating:
                        if (!m_enumerator.MoveNext()) {
                            m_state = State.Finished;
                            return false;
                        }
                        if (m_predicate(arg1:m_enumerator.Current, arg2:m_taken)) {
                            ++m_taken;
                            return true;
                        }
                        m_state = State.Finished;
                        return false;
                    case State.Finished:
                        return false;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            m_state = State.Default;
            m_taken = 0;
        }
    }
    
}