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
        public TSource Current {
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
        private TEnumerator m_enumerator;
        private readonly TSource m_element;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public AppendEnumerator (in TEnumerator enumerator, TSource element) {
            m_enumerator = enumerator;
            m_element = element;
            m_state = State.Default;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
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
        public void Reset () {
            m_enumerator.Reset();
            m_state = State.Default;
        }
    }
    
}