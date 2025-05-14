using System.Text.Json.Serialization;

namespace Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SprintAnswerType
    {
        EveningStandUpDrive,
        EveningStandUpLive,
        EveningStandUpPleasure,
        EveningStandUpWinnings,
        ReflectionRegularCorrection,
        ReflectionRegularMyStatus,
        ReflectionRegularOrbits,
        ReflectionRegularWhatINotDoing,
        ReflectionRegularWhatIDoing, 
        ReflectionWhatIDoing,
        ReflectionIntegrationChanges,
        ReflectionIntegrationActions,
        ReflectionIntegrationAbilities,
        ReflectionIntegrationBeliefs,
        ReflectionIntegrationSelfPerception,
        ReflectionIntegrationOpportunities,
        Status
    }
}
