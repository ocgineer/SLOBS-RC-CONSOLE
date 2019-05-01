using System;
using System.IO;
using System.Xml.Linq;

namespace SLOBSRC
{
    class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            // Debugging arguments
            args = new string[] { "debug" };
#endif
            // Cancel if no function is given
            if (args.Length == 0)
            {
                Console.Write("No function given. Use -h or help for available functions and usage and make sure Streamlabs OBS is running.");
                return;
            }

            // Read config from IP file
            string address = null;
            try
            {
                var exeloc = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                XDocument xml = XDocument.Load(Path.Combine(exeloc, "config.xml"));
                var q = xml.Root.Descendants("ipaddress");
                foreach (string ip in q)
                {
                    address = ip;
                }
            }
            catch (Exception e)
            {
                address = "127.0.0.1";
                Console.Write(e);
            }

            // Control result
            ControlStatus result;

            // Connect to SLOBS
            var connection = new SLOBSConnection(address);
            var status = connection.Connect();
            if (!status.Status)
            {
                Console.WriteLine(status.Message);
                return;
            }

            // function
            switch (args[0].ToLower())
            {
                #region Help

                case "-h":
                case "help":
                    Console.WriteLine("STREAMLABS OBS CONTROL BRIDGE APPLICATION.");
                    Console.WriteLine();
                    Console.WriteLine("function\t\t\t[argument...]");
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("change_scene\t\t\tscene_name [delay]");
                    Console.WriteLine("swap_scenes\t\t\tscene_name delay [scene_name]");
                    Console.WriteLine();
                    Console.WriteLine("visibility_source_active\tsource_name [on|off]");
                    Console.WriteLine("tvisibility_source_active\tsource_name delay [on|onoff|off|offon]");
                    Console.WriteLine();
                    Console.WriteLine("visibility_source_scene\t\tsource_name scene_name [on|off]");
                    Console.WriteLine("tvisibility_source_scene\tsource_name scene_name delay [on|onoff|off|offon]");
                    Console.WriteLine();
                    Console.WriteLine("visibility_folder_active\tfolder_name [on|off]");
                    Console.WriteLine("tvisibility_folder_active\tfolder_name delay [on|onoff|off|offon]");
                    Console.WriteLine();
                    Console.WriteLine("visibility_folder_scene\t\tfolder_name scene_name [on|off]");
                    Console.WriteLine("tvisibility_folder_scene\tfolder_name scene_name delay [on|onoff|off|offon]");
                    Console.WriteLine();
                    Console.WriteLine("start_streaming");
                    Console.WriteLine("stop_streaming");
                    Console.WriteLine();
                    Console.WriteLine("start_recording");
                    Console.WriteLine("stop_recording");
                    Console.WriteLine();
                    Console.WriteLine("start_replaybuffer");
                    Console.WriteLine("stop_replaybuffer");
                    Console.WriteLine("save_replaybuffer");
                    Console.WriteLine("save_replaybuffer_swap\t\tscene_name [duration-offset]");
                    Console.WriteLine();
                    Console.WriteLine();
                    break;

                #endregion

                #region Scenes

                /* change_scene scene_name [delay] */
                case "change_scene":
                    if (args.Length >= 2)
                    {
                        // Set delay
                        int delay = 0;
                        if (args.Length >= 3)
                        {
                            if (!int.TryParse(args[2], out delay))
                            {
                                Console.Write("Warning: Delay for function change_scene is not a valid number, defaulting to 0 seconds.");
                            }
                        }

                        // Swap to scene
                        result = Control.ChangeToScene(connection, args[1], delay);
                        if (!result.Success)
                        {
                            Console.WriteLine($"Error: {result.Message} for function change_scene.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function change_scene.");
                    }
                    break;

                /* swap_scenes scene_name delay [scene_name] */
                case "swap_scenes":
                    if (args.Length >= 3)
                    {
                        // Set delay
                        if (!int.TryParse(args[2], out int delay))
                        {
                            Console.Write("Warning: Delay for function swap_scenes is not a valid number, defaulting to 5 seconds.");
                            delay = 5;
                        }

                        // Set return scene if given
                        string returnscene = (args.Length >= 4) ? args[3] : null;

                        // Swap scene
                        result = Control.SwapScenes(connection, args[1], delay, returnscene);
                        if (!result.Success)
                        {
                            Console.WriteLine($"Error: {result.Message} for function swap_scenes.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for swap_scenes.");
                    }
                    break;

                #endregion

                #region Visibility Item Active

                /* visibility_source_active source_name [on|off] */
                case "visibility_source_active":
                    if (args.Length >= 2)
                    {
                        // Set visibility mode
                        EShowMode mode = EShowMode.ON;
                        if (args.Length >= 3)
                        {
                            switch (args[2].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function visibility_source_active not correct, use `on`, or `off`, defaulting to on.");
                                    break;
                            }
                        }

                        // Set scene item visibility
                        result = Control.VisibilitySceneItem(connection, args[1], mode, 0);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function visibility_source_active.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function visibility_source_active.");
                    }
                    break;

                /* tvisibility_source_active source_name delay [on|onoff|off|offon] */
                case "tvisibility_source_active":
                    if (args.Length >= 3)
                    {
                        // Set delay
                        if (!int.TryParse(args[2], out int delay))
                        {
                            Console.Write("Warning: Delay for function tvisibility_source_active is not a valid number, defaulting to 5 seconds.");
                            delay = 5;
                        }

                        // Set visibility mode
                        EShowMode mode = EShowMode.ONOFF;
                        if (args.Length >= 4)
                        {
                            switch (args[3].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                case "onoff":
                                    mode = EShowMode.ONOFF;
                                    break;
                                case "offon":
                                    mode = EShowMode.OFFON;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function tvisibility_source_active not correct, use `onoff`, `offon`, `on`, or `off`, defaulting to onoff.");
                                    break;
                            }
                        }

                        // Set scene item visibility
                        result = Control.VisibilitySceneItem(connection, args[1], mode, delay);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function tvisibility_source_active");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function tvisibility_source_active.");
                    }
                    break;

                #endregion

                #region Visibility Item Scene

                /* visibility_source_scene source_name scene_name [on|off] */
                case "visibility_source_scene":
                    if (args.Length >= 3)
                    {
                        // Set visibility mode
                        EShowMode mode = EShowMode.ON;
                        if (args.Length >= 4)
                        {
                            switch (args[3].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function visibility_source_scene not correct, use `on` or `off`, defaulting to on.");
                                    break;
                            }
                        }

                        // Set scene item visibility
                        result = Control.VisibilitySceneItem(connection, args[1], mode, 0, args[2]);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function visibility_source_scene.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function visibility_source_scene.");
                    }
                    break;

                /* tvisibility_source_scene source_name scene_name delay [on|onoff|off|offon] */
                case "tvisibility_source_scene":
                    if (args.Length >= 4)
                    {
                        // Set delay
                        if (!int.TryParse(args[3], out int delay))
                        {
                            Console.Write("Warning: Delay for function tvisibility_source_scene is not valid, defaulting to 5 seconds.");
                            delay = 5;
                        }

                        // Set visibility mode
                        EShowMode mode = EShowMode.ONOFF;
                        if (args.Length >= 5)
                        {
                            switch (args[4].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                case "onoff":
                                    mode = EShowMode.ONOFF;
                                    break;
                                case "offon":
                                    mode = EShowMode.OFFON;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function tvisibility_source_scene not correct, use `onoff`, `offon`, `on`, or `off`, defaulting to onoff.");
                                    break;
                            }
                        }

                        // Set scene item visibility
                        result = Control.VisibilitySceneItem(connection, args[1], mode, delay, args[2]);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function tvisibility_source_scene.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function tvisibility_source_scene.");
                    }
                    break;

                #endregion

                #region Visibility Folders Active

                /* visibility_folder_active folder_name [on|off] */
                case "visibility_folder_active":
                    if (args.Length >= 2)
                    {
                        // Set visibility mode
                        EShowMode mode = EShowMode.ON;
                        if (args.Length >= 3)
                        {
                            switch (args[2].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for visibility_folder_active not correct, use `on`, or `off`, defaulting to on.");
                                    break;
                            }
                        }

                        // Set scene folder visibility
                        result = Control.VisibilitySceneFolder(connection, args[1], mode, 0);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function visibility_folder_active.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function visibility_folder_active.");
                    }
                    break;

                /* tvisibility_folder_active folder_name delay [on|onoff|off|offon] */
                case "tvisibility_folder_active":
                    if (args.Length >= 3)
                    {
                        // Set delay
                        if (!int.TryParse(args[2], out int delay))
                        {
                            Console.Write("Warning: Delay for function tvisibility_folder_active is not valid, defaulting to 5 seconds.");
                            delay = 5;
                        }

                        // Set visibility mode
                        EShowMode mode = EShowMode.ONOFF;
                        if (args.Length >= 4)
                        {
                            switch (args[3].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                case "onoff":
                                    mode = EShowMode.ONOFF;
                                    break;
                                case "offon":
                                    mode = EShowMode.OFFON;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function tvisibility_folder_active not correct, use `onoff`, `offon`, `on`, or `off`, defaulting to onoff.");
                                    break;
                            }
                        }

                        // Set scene folder visibility
                        result = Control.VisibilitySceneFolder(connection, args[1], mode, delay);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function tvisibility_folder_active.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function tvisibility_folder_active.");
                    }
                    break;

                #endregion

                #region Visibility Folders Scene

                /* visibility_folder_scene folder_name scene_name [on|off] */
                case "visibility_folder_scene":
                    if (args.Length >= 3)
                    {
                        // Set visibility mode
                        EShowMode mode = EShowMode.ON;
                        if (args.Length >= 4)
                        {
                            switch (args[3].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function visibility_folder_scene not correct, use `on` or `off`, defaulting to on.");
                                    break;
                            }
                        }

                        // Set scene folder visibility
                        result = Control.VisibilitySceneFolder(connection, args[1], mode, 0, args[2]);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function visibility_folder_scene.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function visibility_folder_scene.");
                    }
                    break;

                /* tvisibility_folder_scene folder_name scene_name delay [on|onoff|off|offon] */
                case "tvisibility_folder_scene":
                    if (args.Length >= 4)
                    {
                        // Set delay
                        if (!int.TryParse(args[3], out int delay))
                        {
                            Console.Write("Warning: Delay for function tvisibility_folder_scene is invalid, defaulting to 5 seconds.");
                            delay = 5;
                        }

                        // Set visibility mode
                        EShowMode mode = EShowMode.ONOFF;
                        if (args.Length >= 5)
                        {
                            switch (args[4].ToLower())
                            {
                                case "on":
                                    mode = EShowMode.ON;
                                    break;
                                case "off":
                                    mode = EShowMode.OFF;
                                    break;
                                case "onoff":
                                    mode = EShowMode.ONOFF;
                                    break;
                                case "offon":
                                    mode = EShowMode.OFFON;
                                    break;
                                default:
                                    Console.Write("Warning: Show argument for function tvisibility_folder_scene not correct, use `onoff`, `offon`, `on`, or `off`, defaulting to onoff.");
                                    break;
                            }
                        }

                        // Set scene folder visibility
                        result = Control.VisibilitySceneFolder(connection, args[1], mode, delay, args[2]);
                        if (!result.Success)
                        {
                            Console.Write($"Error: {result.Message} for function tvisibility_folder_scene.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function tvisibility_folder_scene.");
                    }
                    break;

                #endregion

                #region StreamlabsOBS States

                case "start_streaming":
                    result = Control.StartStreaming(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function start_streaming.");
                    break;

                case "stop_streaming":
                    result = Control.StopStreaming(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function stop_streaming.");
                    break;

                case "start_recording":
                    result = Control.StartRecording(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function start_recording.");
                    break;

                case "stop_recording":
                    result = Control.StopRecording(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function stop_recording.");
                    break;

                case "start_replaybuffer":
                    result = Control.StartReplayBuffer(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function start_replaybuffer.");
                    break;

                case "stop_replaybuffer":
                    result = Control.StopReplayBuffer(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function stop_replaybuffer.");
                    break;

                case "save_replaybuffer":
                    result = Control.SaveReplayBuffer(connection);
                    if (!result.Success) Console.WriteLine($"Error: {result.Message} for function save_replaybuffer.");
                    break;

                /* save_replaybuffer_swap scene_name [duration-offset] */
                case "save_replaybuffer_swap":
                    if (args.Length >= 2)
                    {
                        int offset = 3;
                        if (args.Length >= 3)
                        {
                            // Set duration-offset
                            if (!int.TryParse(args[2], out offset))
                            {
                                Console.Write("Warning: Duration-offset for function save_replaybuffer_swap is not a valid number, defaulting to 3 seconds.");
                            }
                        }

                        // Save replay and swap to given replay scene
                        result = Control.SaveReplayBufferSwapScenes(connection, args[1], offset);
                        if (!result.Success)
                        {
                            Console.WriteLine($"Error: {result.Message} for function save_replaybuffer_swap.");
                        }
                    }
                    else
                    {
                        Console.Write("Error: Insufficient arguments supplied for function save_replaybuffer_swap.");
                    }
                    break;

                #endregion

                #region Debug

#if DEBUG
                case "debug":

                    break;
#endif

                #endregion

                default:
                    Console.Write("Error: Function does not exist.");
                    break;
            }

            // Close connection to SLOBS
            connection.Close();
        }
    }
}
