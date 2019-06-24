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

    public struct SkipWhileIndexedEnumerator<TEnumerator, TSource> :
        IAdaptableEnumerator<TSource>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Types
        //--------------------------------------------------------------------------------------------------------------
        private enum State {
            Default,
            Skipping,
            Enumerating,
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
                    case State.Default:
                    case State.Skipping:
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
        private readonly Func<TSource, int, bool> m_predicate;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public SkipWhileIndexedEnumerator (in TEnumerator enumerator, Func<TSource, int, bool> predicate) {
            m_enumerator = enumerator;
            m_predicate = predicate;
            m_state = State.Default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            for (var skipped = 0;;) {
                switch (m_state) {
                    case State.Default:
                        m_state = State.Skipping;
                        break;
                    case State.Skipping:
                        if (!m_enumerator.MoveNext()) {
                            m_state = State.Finished;
                            return false;
                        }
                        if (!m_predicate(arg1:m_enumerator.Current, arg2:skipped)) {
                            m_state = State.Enumerating;
                            return true;
                        }
                        ++skipped;
                        break;
                    case State.Enumerating:
                        if (!m_enumerator.MoveNext()) {
                            m_state = State.Finished;
                            return false;
                        }
                        return true;
                    case State.Finished:
                        return false;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_enumerator.Reset();
            m_state = State.Default;
        }
    }
    
}