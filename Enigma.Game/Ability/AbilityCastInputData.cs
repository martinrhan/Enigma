﻿using Enigma.Common.Math;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Enigma.Game {
    public class AbilityCastInputData {
        public Vector2 TargetPoint { get; init; }
        public Vector2 SecondTargetPoint { get; init; }
        public GameBody TargetGameBody { get; init; }
    }

    public class AbilityCastInputDataCollection {
        private AbilityCastInputData[] array = new AbilityCastInputData[1];

        internal void Update(IEnumerable<ValueTuple<int, AbilityCastInputData>> inputDatas) {
            foreach (var inputData in inputDatas) {
                array[inputData.Item1] = inputData.Item2;
            }
        }

        public AbilityCastInputData this[int index] {
            get {
                if (index >= array.Length) {
                    Array.Resize(ref array, index + 1);
                }
                return array[index];
            }
        }

        internal void ResizeIfSmaller(int count) {
            if (array.Length < count) {
                Array.Resize(ref array, count);
            }
        }
    }
}