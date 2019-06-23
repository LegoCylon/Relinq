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

    public struct AppendEnumerator<TEnumerator, TSource> : 
        IAdaptableEnumerator<TSource>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Types
        //--------------------------------------------------------------------------------------------------------------
        private enum State {
            Default,
            UsingEnumerator,
            UsingElement,
            Finished,
        }

        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<AppendEnumerator<TEnumerator, TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<AppendEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref AppendEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref AppendEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref AppendEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref AppendEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current {
            get {
                switch (m_state) {
                    case State.UsingEnumerator:
                        return m_enumerator.Current;
                    case State.UsingElement:
                        return m_element;
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
        private readonly TSource m_element;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        public static EnumerableAdapter<AppendEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            TSource element
        ) => 
            new EnumerableAdapter<AppendEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<AppendEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new AppendEnumerator<TEnumerator, TSource>(enumerator:enumerator, element:element)
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        private AppendEnumerator (in EnumeratorAdapter<TEnumerator, TSource> enumerator, TSource element) {
            m_enumerator = enumerator;
            m_element = element;
            m_state = State.Default;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            switch (m_state) {
                case State.Default:
                case State.UsingEnumerator:
                    m_state = State.UsingEnumerator;
                    if (m_enumerator.MoveNext()) {
                        return true;
                    }
                    m_state = State.UsingElement;
                    return true;
                case State.UsingElement:
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
            m_enumerator.Reset();
            m_state = State.Default;
        }
    }
    
}