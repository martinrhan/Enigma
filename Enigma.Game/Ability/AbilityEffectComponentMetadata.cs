using System.Collections.Generic;

namespace Enigma.Game {
    public partial class AbilityPhaseStageTemplate {
        public class AbilityEffectComponentMetadata {
            public string Id { get; init; }
            public AbilityEffectComponentFactory Factory { get; init; }
            public IReadOnlyList<ArgumentDataSourceInfo> InvokeArgumentDataSourceInfoList { get; init; }
            public IReadOnlyList<InvokeArgumentMetadata> InvokeArgumentMetadataList { get; init; }

            public struct ArgumentDataSourceInfo {
                public string SourceComponentId { get; init; }
                public string SourcePropertyId { get; init; }
                public string InExpressionId { get; init; }
            }
            public struct InvokeArgumentMetadata {
                public string ArgumentId { get; init; }
                public string ConvertExpression { get; init; }
            }
        }
    }
}
