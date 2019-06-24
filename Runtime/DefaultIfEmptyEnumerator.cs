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
            Fallback,
            Finished,
        }
        
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current {
            get {
                switch (m_state) {
                    case State.Enumerating:
                        return m_enumerator.Current;
                    case State.Fallback:
                        return default;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in TEnumerator enumerator
        ) =>
            new EnumerableAdapter<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new DefaultIfEmptyEnumerator<TEnumerator, TSource>(enumerator:enumerator)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        private DefaultIfEmptyEnumerator (in TEnumerator enumerator) {
            m_enumerator = enumerator;
            m_state = State.Default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            switch (m_state) {
                case State.Default:
                    if (m_enumerator.MoveNext()) {
                        m_state = State.Enumerating;
                        return true;
                    }
                    m_state = State.Fallback;
                    return true;
                case State.Enumerating:
                    if (m_enumerator.MoveNext()) {
                        return true;
                    }
                    m_state = State.Finished;
                    return false;
                case State.Fallback:
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
            m_state = State.Default;
        }
    }
    
}