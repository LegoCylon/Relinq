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

    public struct DefaultIfEmptyEnumerator<TEnumerator, TSource> : 
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
        private static EnumeratorDescription<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource> 
            Description { get; } =
            new EnumeratorDescription<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref DefaultIfEmptyEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref DefaultIfEmptyEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref DefaultIfEmptyEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref DefaultIfEmptyEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator
        ) =>
            new EnumerableAdapter<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new DefaultIfEmptyEnumerator<TEnumerator, TSource>(enumerator:enumerator)
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        private DefaultIfEmptyEnumerator (in EnumeratorAdapter<TEnumerator, TSource> enumerator) {
            m_enumerator = enumerator;
            m_state = State.Default;
            Current = default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            switch (m_state) {
                case State.Default:
                    if (m_enumerator.MoveNext()) {
                        Current = m_enumerator.Current;
                        m_state = State.Enumerating;
                        return true;
                    }
                    m_state = State.Finished;
                    return true;
                case State.Enumerating:
                    if (m_enumerator.MoveNext()) {
                        Current = m_enumerator.Current;
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

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            Current = default;
            m_state = State.Default;
        }
    }
    
}