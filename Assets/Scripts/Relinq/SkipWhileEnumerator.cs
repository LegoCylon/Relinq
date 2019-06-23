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

    public struct SkipWhileEnumerator<TEnumerator, TSource> :
        IAdaptedEnumerator<TSource>
        where TEnumerator : IAdaptedEnumerator<TSource>
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
        private static EnumeratorDescription<SkipWhileEnumerator<TEnumerator, TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<SkipWhileEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref SkipWhileEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref SkipWhileEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref SkipWhileEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref SkipWhileEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current {
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
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<TSource, bool> m_predicate;
        private State m_state;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<SkipWhileEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, bool> predicate
        ) =>
            new EnumerableAdapter<SkipWhileEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<SkipWhileEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new SkipWhileEnumerator<TEnumerator, TSource>(
                        enumerator:enumerator, 
                        predicate:predicate
                    )
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private SkipWhileEnumerator (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            Func<TSource, bool> predicate
        ) {
            m_enumerator = enumerator;
            m_predicate = predicate;
            m_state = State.Default;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            for (;;) {
                switch (m_state) {
                    case State.Default:
                        m_state = State.Skipping;
                        break;
                    case State.Skipping:
                        if (!m_enumerator.MoveNext()) {
                            m_state = State.Finished;
                            return false;
                        }
                        if (!m_predicate(arg:m_enumerator.Current)) {
                            m_state = State.Enumerating;
                            return true;
                        }
                        break;
                    case State.Enumerating:
                        if (!m_enumerator.MoveNext()) {
                            m_state = State.Finished;
                            return false;
                        }
                        m_state = State.Finished;
                        return true;
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
        }
    }
    
}