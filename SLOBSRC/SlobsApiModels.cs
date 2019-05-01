using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SLOBSRC
{
    enum EStreamingState
    {
        Ending,
        Live,
        Offline,
        Reconnecting,
        Starting
    }

    enum ERecordingState
    {
        Offline,
        Recording,
        Starting,
        Stopping
    }

    enum EReplayBufferState
    {
        Running,
        Stopping,
        Offline,
        Saving
    }

    enum ESceneNodeType
    {
        Item,
        Folder
    }

    enum ENotificationType
    {
        INFO,
        SUCCESS,
        WARNING
    }

    /// <summary>
    /// ISettingsSubCategory
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_settings_settings_api_.isettingssubcategory.html
    /// </summary>
    struct Settings
    {
        [JsonProperty("nameSubCategory")]
        public string NameSubCategory { get; private set; }

        [JsonProperty("parameters")]
        public List<SettingsParameters> Parameters { get; private set; }
    }

    /// <summary>
    /// ISettingsSubCategory TFormData
    /// </summary>
    struct SettingsParameters
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("currentValue")]
        public string CurrentValue { get; private set; }
    }

    /// <summary>
    /// ISceneAPI
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_scenes_scenes_api_.isceneapi.html
    /// </summary>
    struct SceneAPI
    {
        [JsonProperty("_type")]
        public string _Type { get; private set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("nodes")]
        public List<SceneNodeModel> Nodes { get; private set; }
    }

    /// <summary>
    /// ISceneItem | ISceneItemFolder
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_scenes_scenes_api_.isceneitem.html
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_scenes_scenes_api_.isceneitemfolder.html
    /// </summary>
    struct SceneNodeModel
    {
        [JsonProperty("childrenIds")]
        public List<string> ChildrenIds { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("locked")]
        public bool Locked { get; private set; }

        [JsonProperty("obsSceneItemId")]
        public int ObsSceneItemId { get; private set; }

        [JsonProperty("parentId")]
        public string ParentId { get; private set; }

        [JsonProperty("sceneId")]
        public string SceneId { get; private set; }

        [JsonProperty("sceneItemId")]
        public string SceneItemId { get; private set; }

        [JsonProperty("sceneNodeType")]
        public ESceneNodeType SceneNodeType { get; private set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; private set; }

        //[JsonProperty("transform")]
        //public Transform Transform { get; private set; }

        [JsonProperty("visible")]
        public bool Visible { get; private set; }
    }

    /// <summary>
    /// ISceneItemAPI
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_scenes_scenes_api_.isceneitemapi.html
    /// </summary>
    struct SceneItemAPI
    {
        [JsonProperty("_type")]
        public string _Type { get; private set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; private set; }

        [JsonProperty("childrenIds")]
        public List<string> ChildrenIds { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("locked")]
        public bool Locked { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("obsSceneItemId")]
        public int ObsSceneItemId { get; private set; }

        [JsonProperty("parentId")]
        public string ParentId { get; private set; }

        [JsonProperty("sceneId")]
        public string SceneId { get; private set; }

        [JsonProperty("sceneItemId")]
        public string SceneItemId { get; private set; }

        [JsonProperty("sceneNodeType")]
        public ESceneNodeType SceneNodeType { get; private set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; private set; }

        //[JsonProperty("transform")]
        //public Transform Transform { get; private set; }

        [JsonProperty("visible")]
        public bool Visible { get; private set; }
    }

    /// <summary>
    /// ISceneItemFolderAPI
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_scenes_scenes_api_.isceneitemfolderapi.html
    /// </summary>
    struct SceneItemFolderAPI
    {
        [JsonProperty("_type")]
        public string _Type { get; private set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; private set; }

        [JsonProperty("childrenIds")]
        public List<string> ChildrenIds { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("parentId")]
        public string ParentId { get; private set; }

        [JsonProperty("sceneId")]
        public string SceneId { get; private set; }

        [JsonProperty("sceneNodeType")]
        public ESceneNodeType SceneNodeType { get; private set; }
    }

    /// <summary>
    /// ISelection
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_selection_selection_api_.iselection.html
    /// </summary>
    struct Selection
    {
        [JsonProperty("_type")]
        public string _Type { get; private set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; private set; }

        [JsonProperty("selectedIds")]
        public List<string> SelectedIds { get; private set; }

        [JsonProperty("lastSelectedId")]
        public string LastSelectedId { get; private set; }
    }

    /// <summary>
    /// ISourceAPI
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_sources_sources_api_.isourceapi.html
    /// </summary>
    struct SourceAPI
    {
        [JsonProperty("_type")]
        public string _Type { get; private set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; private set; }

        [JsonProperty("audio")]
        public bool Audio { get; private set; }

        [JsonProperty("channel")]
        public int Channel { get; private set; }

        [JsonProperty("doNotDuplicate")]
        public bool DoNotDuplicate { get; private set; }

        [JsonProperty("height")]
        public int Height { get; private set; }

        [JsonProperty("muted")]
        public bool Muted { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("video")]
        public bool Video { get; private set; }

        [JsonProperty("width")]
        public int Width { get; private set; }
    }

    /// <summary>
    /// IServiceState
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_streaming_streaming_api_.istreamingservicestate.html
    /// </summary>
    struct StreamingServiceState
    {
        [JsonProperty("recordingStatus")]
        public ERecordingState RecordingStatus { get; private set; }

        [JsonProperty("recordingStatusTime")]
        public string RecordingStatusTime { get; private set; }

        [JsonProperty("streamingStatus")]
        public EStreamingState StreamingStatus { get; private set; }

        [JsonProperty("streamingStatusTime")]
        public string StreamingStatusTime { get; private set; }

        [JsonProperty("replayBufferStatus")]
        public EReplayBufferState ReplayBufferStatus { get; private set; }

        [JsonProperty("replayBufferStatusTime")]
        public string ReplayBufferTime { get; private set; }
    }

    /// <summary>
    /// INotification
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_notifications_notifications_api_.inotification.html
    /// </summary>
    struct Notification
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("code")]
        public string Code { get; private set; }

        [JsonProperty("data")]
        public object Data { get; private set; }

        [JsonProperty("unread")]
        public bool Unread { get; private set; }

        [JsonProperty("date")]
        public long Date { get; private set; }

        [JsonProperty("type")]
        public ENotificationType Type { get; private set; }

        [JsonProperty("lifeTime")]
        public int LifeTime { get; private set; }

        [JsonProperty("showTime")]
        public bool ShowTime { get; private set; }

        [JsonProperty("playSound")]
        public bool PlaySound { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }
    }

    /// <summary>
    /// INotificationOptions
    /// https://stream-labs.github.io/streamlabs-obs-api-docs/docs/interfaces/_notifications_notifications_api_.inotificationoptions.html
    /// </summary>
    class NotifictionOptions
    {
        public NotifictionOptions(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Message of the notification, required set via constructor.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; private set; }

        /// <summary>
        /// Type of the notification as <seealso cref="ENotificationType"/>, default INFO.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ENotificationType Type { get; set; } = ENotificationType.INFO;

        /// <summary>
        /// Play sound not push notification, defaults true.
        /// </summary>
        [JsonProperty("playSound")]
        public bool PlaySound { get; set; } = true;

        /// <summary>
        /// Time of status bar notification visibility in miliseconds, default 8000
        /// </summary>
        [JsonProperty("lifeTime")]
        public int LifeTime { get; set; } = 8000;

        /// <summary>
        /// Show the time with the message in the status bar notification, default false.
        /// </summary>
        [JsonProperty("showTime")]
        public bool ShowTime { get; set; } = false;

        /// <summary>
        /// Indicate if this new pushed notification should be marked as read, default true.
        /// </summary>
        [JsonProperty("unread")]
        public bool Unread { get; set; } = true;
    }
}
