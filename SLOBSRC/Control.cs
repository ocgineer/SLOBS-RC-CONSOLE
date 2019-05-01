using System.Collections.Generic;
using System.Threading;

namespace SLOBSRC
{
    enum EShowMode
    {
        ON,
        OFF,
        ONOFF,
        OFFON
    }

    struct ControlStatus
    {
        public bool Success;
        public string Message;
    }

    class Control
    {
        #region Streaming

        /// <summary>
        /// Start the stream in the connected Streamlabs OBS.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus StartStreaming(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // State is offline or ending
            if (state.StreamingStatus == EStreamingState.Offline || state.StreamingStatus == EStreamingState.Ending)
            {
                // Start stream
                new Request<object>("toggleStreaming", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */
                return new ControlStatus
                {
                    Success = true
                };
            }
            // Stream is live, starting, or reconnecting
            else
            {
                SendNotification(con, "Stream is already live or is reconnecting.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Stream is already live or is reconnecting"
                };
            }
        }

        /// <summary>
        /// Stop the stream in the connected Streamlabs OBS.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus StopStreaming(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // State is live, starting, or reconnecting
            if (state.StreamingStatus == EStreamingState.Live || state.StreamingStatus == EStreamingState.Starting || state.StreamingStatus == EStreamingState.Reconnecting)
            {
                // Stop stream
                new Request<object>("toggleStreaming", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */
                return new ControlStatus
                {
                    Success = true
                };
            }
            // Stream is offline or ending
            else
            {
                SendNotification(con, "Stream is already stopped or is stopping.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Stream is already stopped or is stopping"
                };
            }

        }

        #endregion

        #region Recording

        /// <summary>
        /// Start the recording in the connected Streamlabs OBS.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus StartRecording(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // State is offline or stopping
            if (state.RecordingStatus == ERecordingState.Offline || state.RecordingStatus == ERecordingState.Stopping)
            {
                // Start stream
                new Request<object>("toggleRecording", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */
                return new ControlStatus
                {
                    Success = true
                };
            }
            // Stream is live, starting, or reconnecting
            else
            {
                SendNotification(con, "Recording is already started or is starting.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Recording is already started or is starting"
                };
            }

        }

        /// <summary>
        /// Stop the recording in the connected Streamlabs OBS.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus StopRecording(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // State is recording or starting
            if (state.RecordingStatus == ERecordingState.Recording || state.RecordingStatus == ERecordingState.Starting)
            {
                // Start stream
                new Request<object>("toggleRecording", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */
                return new ControlStatus
                {
                    Success = true
                };
            }
            else
            {
                // Stream is live, starting, or reconnecting
                SendNotification(con, "Recording is already stopped or is stopping.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Recording is already stopped or is stopping"
                };
            }
        }

        #endregion

        #region Replay Buffer

        /// <summary>
        /// Start the replay buffer.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus StartReplayBuffer(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // Replay Buffer is offline or is stopping
            if (state.ReplayBufferStatus == EReplayBufferState.Offline || state.ReplayBufferStatus == EReplayBufferState.Stopping)
            {
                // Start Replay Buffer
                new Request<object>("startReplayBuffer", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */

                return new ControlStatus
                {
                    Success = true
                };
            }
            // Replay Buffer is already running or in saving state
            else
            {
                SendNotification(con, "Replay Buffer is already running.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Replay Buffer is already running"
                };
            }
        }

        /// <summary>
        /// Stop the replay buffer.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus StopReplayBuffer(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // Replay Buffer is running or in saving state
            if (state.ReplayBufferStatus == EReplayBufferState.Running || state.ReplayBufferStatus == EReplayBufferState.Saving)
            {
                // Start Replay Buffer
                new Request<object>("stopReplayBuffer", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */

                return new ControlStatus
                {
                    Success = true
                };
            }
            // Replay Buffer is already offline or is stopping
            else
            {
                SendNotification(con, "Replay Buffer is already stopped or is stopping.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Replay Buffer is already stopped or is stopping"
                };
            }
        }

        /// <summary>
        /// Save the replay buffer.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus SaveReplayBuffer(IConnection con)
        {
            // Get current streaming state
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;

            // Save when replay buffer is saving
            if (state.ReplayBufferStatus == EReplayBufferState.Running)
            {
                new Request<object>("saveReplay", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */
                return new ControlStatus
                {
                    Success = true
                };
            }
            else
            {
                SendNotification(con, "Replay Buffer is stopped or is already saving.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Replay Buffer is stopped or is already saving"
                };
            }
        }

        /// <summary>
        /// Save running replay buffer and change to given scene when done saving.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <param name="replayscene">Scene to change to after saving the replay buffer is done.</param>
        /// <param name="offset">Delay offset for the buffer duration to move back to origional scene.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus SaveReplayBufferSwapScenes(IConnection con, string replayscene, int offset = 3)
        {
            // Get length of replay buffer from settings
            int bufferlength = 0;
            var settings = new Request<List<Settings>>("getSettingsFormData", "SettingsService", new object[] { "Output" }).GetResponse(con).Result;
            foreach (Settings setting in settings)
            {
                if (setting.NameSubCategory == "Replay Buffer")
                {
                    foreach (SettingsParameters parameter in setting.Parameters)
                    {
                        if (parameter.Name == "RecRBTime")
                        {
                            if (int.TryParse(parameter.CurrentValue, out bufferlength))
                            {
                                // Found Replay Buffer time
                                break;
                            }
                            else
                            {
                                // Unable to find Replay Buffer time
                                SendNotification(con, "Unable to find the Replay Buffer length from settings.");
                                return new ControlStatus
                                {
                                    Success = false,
                                    Message = "Unable to find the Replay Buffer length from settings"
                                };
                            }
                        }
                    }

                    // Found Replay Buffer settings
                    break;
                }
            }

            // Get current streaming state and return if replay is stopped or is stopping
            StreamingServiceState state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;
            if (state.ReplayBufferStatus == EReplayBufferState.Offline || state.ReplayBufferStatus == EReplayBufferState.Stopping)
            {
                SendNotification(con, "Replay Buffer is not running or is stopping.");
                return new ControlStatus
                {
                    Success = false,
                    Message = "Replay Buffer is not running or is stopping"
                };
            }

            // If replay buffer is running save the replay skip if already saving
            if (state.ReplayBufferStatus == EReplayBufferState.Running)
            {
                new Request<object>("saveReplay", "StreamingService").GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */
            }

            do // Wait until replay buffer is done saving
            {
                Thread.Sleep(200);
                state = new Request<StreamingServiceState>("getModel", "StreamingService").GetResponse(con).Result;
            } while (state.ReplayBufferStatus == EReplayBufferState.Saving);

            // Get scenes in current active scene collection
            var scenes = new Request<List<SceneAPI>>("getScenes", "ScenesService").GetResponse(con);
            foreach (SceneAPI s in scenes.Result)
            {
                // Found targeted replay scene
                if (s.Name == replayscene)
                {
                    // Get current active scene
                    SceneAPI returnScene = new Request<SceneAPI>("activeScene", "ScenesService").GetResponse(con).Result;

                    // Check if target scene is not same as current scene
                    if (s.Id != returnScene.Id)
                    {
                        // Make targeted replay scene active
                        new Request<object>("makeSceneActive", "ScenesService", new object[] { s.Id }).GetResponse(con);
                        /* TODO: return is not bool due to a bug in SLOBS external API */

                        // Wait for buffer length minus duration offset, minimum 5 seconds
                        int seconds = (bufferlength - offset > 5) ? (bufferlength - offset) : 5;
                        Thread.Sleep(seconds * 1000);

                        // Swap back to previous active scene
                        new Request<object>("makeSceneActive", "ScenesService", new object[] { returnScene.Id }).GetResponse(con);
                        /* TODO: return is not bool due to a bug in SLOBS external API */
                    }

                    return new ControlStatus
                    {
                        Success = true
                    };
                }
            }

            // Targted scene was not found
            SendNotification(con, $"Saved the Replay Buffer but could not find the targeted replay scene '{replayscene}'.");
            return new ControlStatus
            {
                Success = false,
                Message = $"Saved the Replay Buffer but could not find the targeted scene '{replayscene}'"
            };

        }

        #endregion

        #region Scenes

        /// <summary>
        /// Change to scene in active scene collection.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <param name="scene">Name of the scene to set active.</param>
        /// <param name="delay">Delay in seconds before changing scenes.</param>
        /// <returns>Success</returns>
        internal static ControlStatus ChangeToScene(IConnection con, string scene, int delay = 0)
        {
            // Get scenes in current active scene collection
            var scenes = new Request<List<SceneAPI>>("getScenes", "ScenesService").GetResponse(con).Result;
            foreach (SceneAPI s in scenes)
            {
                // Found targeted scene
                if (s.Name == scene)
                {
                    // Swap to targeted scene
                    if (delay > 0) Thread.Sleep(delay * 1000);
                    new Request<object>("makeSceneActive", "ScenesService", new object[] { s.Id }).GetResponse(con);
                    /* TODO: return is not bool due to a bug in SLOBS external API */
                    return new ControlStatus
                    {
                        Success = true
                    };
                }
            }

            // Targeted scene not found
            SendNotification(con, $"Scene '{scene}' not found to change to.");
            return new ControlStatus
            {
                Success = false,
                Message = $"Scene '{scene}' not found to change to"
            };
        }

        /// <summary>
        /// Change to scene in the active scene collection and after a set timeout swap
        /// to another given scene or back to the previous active scene if none given.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <param name="first_scene">Target scene to change to.</param>
        /// <param name="delay">Delay in seconds before changing scenes.</param>
        /// <param name="second_scene">Optionally return scene to change to, previous active scene if omitted.</param>
        /// <returns>Success</returns>
        internal static ControlStatus SwapScenes(IConnection con, string targetscene, int delay, string returnscene = null)
        {
            // Get scenes from current active scene collection
            var scenes = new Request<List<SceneAPI>>("getScenes", "ScenesService").GetResponse(con).Result;

            // Get current active scene if no return scene given
            string returnId = null;
            if (returnscene == null)
            {
                // Get current Active scene Id
                returnId = new Request<SceneAPI>("activeScene", "ScenesService").GetResponse(con).Result.Id;
            }

            // Find targeted scene and return scene if given
            string targetId = null;
            foreach (SceneAPI s in scenes)
            {
                // Found targeted scene
                if (s.Name == targetscene)
                {
                    targetId = s.Id;
                    if (returnId != null) break;
                }

                // Found return scene
                if (returnscene != null && s.Name == returnscene)
                {
                    returnId = s.Id;
                    if (targetId != null) break;
                }
            }

            if (targetId != null && returnId != null)
            {
                // Swap to targeted scene
                new Request<object>("makeSceneActive", "ScenesService", new object[] { targetId }).GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */

                // Sleep
                if (delay > 0) Thread.Sleep(delay * 1000);

                // Swap to return scene
                new Request<object>("makeSceneActive", "ScenesService", new object[] { returnId }).GetResponse(con);
                /* TODO: return is not bool due to a bug in SLOBS external API */

                return new ControlStatus
                {
                    Success = true
                };
            }
            else
            {
                if (targetId == null)
                {
                    SendNotification(con, $"Scene '{targetscene}' not found to change to.");
                    return new ControlStatus
                    {
                        Success = false,
                        Message = $"Scene '{targetscene}' not found to change to"
                    };
                }
                else
                {
                    SendNotification(con, $"Scene '{returnscene}' not found to return to.");
                    return new ControlStatus
                    {
                        Success = false,
                        Message = $"Scene '{returnscene}' not found to return to"
                    };
                }
            }
        }

        #endregion

        #region Visibility Item/Folder

        /// <summary>
        /// Set the visibility of an item (source) in a targeted scene or current active scene if not given.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <param name="item">Targeted item (source) to set the visibility on.</param>
        /// <param name="mode">Visibility mode with delay between or before.</param>
        /// <param name="delay">Delay in seconds of the visibility mode.</param>
        /// <param name="scene">Scene name to find the targeted item (source) in.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus VisibilitySceneItem(IConnection con, string item, EShowMode mode, int delay, string scene = null)
        {
            // Get targetd Scene or current active scene
            string sceneResourceId = null;
            if (scene == null)
            {
                // Get current Active scene resourceId
                sceneResourceId = new Request<SceneAPI>("activeScene", "ScenesService").GetResponse(con).Result.ResourceId;
            }
            else
            {
                // Get scenes in current active scene collection
                var scenes = new Request<List<SceneAPI>>("getScenes", "ScenesService").GetResponse(con);
                foreach (SceneAPI s in scenes.Result)
                {
                    // Found targeted scene
                    if (s.Name == scene)
                    {
                        sceneResourceId = s.ResourceId;
                        break;
                    }
                }

                // Targeted scene not found
                if (sceneResourceId == null)
                {
                    SendNotification(con, $"Scene '{scene}' not found.");
                    return new ControlStatus
                    {
                        Success = false,
                        Message = $"Scene '{scene}' not found"
                    };
                }
                    
            }

            // Get all sceneItems from targeted scene
            var items = new Request<List<SceneItemAPI>>("getItems", sceneResourceId).GetResponse(con);

            // Find targeted item by name
            // In items list instead using getSourcesByName and matching source id's
            foreach (SceneItemAPI i in items.Result)
            {
                // Found targeted item
                if (i.Name == item)
                {
                    // Set visibility of the item
                    switch (mode)
                    {
                        /* TODO: return is not bool due to a bug in SLOBS external API */
                        case EShowMode.OFFON:
                            new Request<object>("setVisibility", i.ResourceId, new object[] { false }).GetResponse(con);
                            Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", i.ResourceId, new object[] { true }).GetResponse(con);
                            break;

                        case EShowMode.ONOFF:
                            new Request<object>("setVisibility", i.ResourceId, new object[] { true }).GetResponse(con);
                            Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", i.ResourceId, new object[] { false }).GetResponse(con);
                            break;

                        case EShowMode.OFF:
                            if (delay > 0) Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", i.ResourceId, new object[] { false }).GetResponse(con);
                            break;

                        case EShowMode.ON:
                            if (delay > 0) Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", i.ResourceId, new object[] { true }).GetResponse(con);
                            break;
                    }

                    // Return success
                    return new ControlStatus
                    {
                        Success = true
                    };
                }
            }

            // Targeted item not found
            if (scene == null)
            {
                SendNotification(con, $"Source '{item}' not found in current scene.");
                return new ControlStatus
                {
                    Success = false,
                    Message = $"Source '{item}' not found in current scene"
                };
            }
            else
            {
                SendNotification(con, $"Source '{item}' not found in scene '{scene}'.");
                return new ControlStatus
                {
                    Success = false,
                    Message = $"Source '{item}' not found in scene '{scene}'"
                };
            }
        }

        /// <summary>
        /// Set the visibility of a folder in a targeted scene or current active scene if not given.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <param name="folder">Targeted folder to set the visibility on.</param>
        /// <param name="mode">Visibility mode with delay between or before.</param>
        /// <param name="delay">Delay in miliseconds of the visibility mode.</param>
        /// <param name="scene">Scene name to find the targeted folder in.</param>
        /// <returns>ControlStatus success</returns>
        internal static ControlStatus VisibilitySceneFolder(IConnection con, string folder, EShowMode mode, int delay, string scene = null)
        {
            // Get targetd Scene or current active scene
            string sceneResourceId = null;
            if (scene == null)
            {
                // Get current Active scene resourceId
                sceneResourceId = new Request<SceneAPI>("activeScene", "ScenesService").GetResponse(con).Result.ResourceId;
            }
            else
            {
                // Get scenes in current active scene collection
                var scenes = new Request<List<SceneAPI>>("getScenes", "ScenesService").GetResponse(con);
                foreach (SceneAPI s in scenes.Result)
                {
                    // Found targeted scene
                    if (s.Name == scene)
                    {
                        sceneResourceId = s.ResourceId;
                        break;
                    }
                }

                // Targeted scene not found
                if (sceneResourceId == null)
                {
                    SendNotification(con, $"Scene '{scene}' not found.");
                    return new ControlStatus
                    {
                        Success = false,
                        Message = $"Scene '{scene}' not found"
                    };
                }
            }

            // Get all sceneItemFolders from targeted scene
            var folders = new Request<List<SceneItemFolderAPI>>("getFolders", sceneResourceId).GetResponse(con);

            // Find targeted folder by name
            foreach (SceneItemFolderAPI f in folders.Result)
            {
                // Found targeted folder
                if (f.Name == folder)
                {
                    // Select everything in the targeted folder
                    var selection = new Request<Selection>("getSelection", f.ResourceId).GetResponse(con).Result;

                    // Set visibility of the selected items
                    switch (mode)
                    {
                        /* TODO: return is not bool due to a bug in SLOBS external API */
                        case EShowMode.OFFON:
                            new Request<object>("setVisibility", selection.ResourceId, new object[] { false }).GetResponse(con);
                            Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", selection.ResourceId, new object[] { true }).GetResponse(con);
                            break;

                        case EShowMode.ONOFF:
                            new Request<object>("setVisibility", selection.ResourceId, new object[] { true }).GetResponse(con);
                            Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", selection.ResourceId, new object[] { false }).GetResponse(con);
                            break;

                        case EShowMode.OFF:
                            if (delay > 0) Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", selection.ResourceId, new object[] { false }).GetResponse(con);
                            break;

                        case EShowMode.ON:
                            if (delay > 0) Thread.Sleep(delay * 1000);
                            new Request<object>("setVisibility", selection.ResourceId, new object[] { true }).GetResponse(con);
                            break;
                    }

                    // Return success
                    return new ControlStatus
                    {
                        Success = true
                    };
                }
            }

            // Targeted folder not found
            if (scene == null)
            {
                SendNotification(con, $"Folder '{folder}' not found in current scene.");
                return new ControlStatus
                {
                    Success = false,
                    Message = $"Folder '{folder}' not found in current scene"
                };
            }
            else
            {
                SendNotification(con, $"Folder '{folder}' not found in scene '{scene}'.");
                return new ControlStatus
                {
                    Success = false,
                    Message = $"Folder '{folder}' not found in scene '{scene}'"
                };
            }
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Push a notification to the Streamlabs OBS notification system.
        /// </summary>
        /// <param name="con">IConnection connection to Streamlabs OBS.</param>
        /// <param name="message">Message to be pushed to Streamlabs OBS (appends "Remote: ").</param>
        /// <param name="type">Type of the notification, default to Warning.</param>
        private static void SendNotification(IConnection con, string message, ENotificationType type = ENotificationType.WARNING)
        {
            
            // Create argument object
            object[] args = new object[]
            {
                new NotifictionOptions($"Remote: {message}")
                {
                    Type = type
                }
            };

            // Send push notifcation to Streamlabs OBS
            new Request<Notification>("push", "NotificationsService", args).GetResponse(con);
        }

        #endregion
    }
}
