using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace SLOBSRC
{
    public struct ConnectionStatus
    {
        public bool Status;
        public string Message;
    }

    public class SLOBSConnection : IConnection
    {
        private readonly string _ip;
        private NamedPipeClientStream _client;
        private StreamReader _reader;
        private StreamWriter _writer;

        public SLOBSConnection(string ip)
        {
            // Init named pipe connection and streams
            _ip = ip;
            _client = new NamedPipeClientStream(_ip, "slobs");
            _reader = new StreamReader(_client);
            _writer = new StreamWriter(_client);
        }

        public ConnectionStatus Connect()
        {
            try
            {
                _client.Connect(2);
                return new ConnectionStatus
                {
                    Status = true
                };
            }
            catch (UnauthorizedAccessException)
            {
                return new ConnectionStatus
                {
                    Status = false,
                    Message = "Unauthorized Access Error. If Streamlabs OBS is running as administrator then also run this application as administrator."
                };
            }
            catch (TimeoutException)
            {
                return new ConnectionStatus
                {
                    Status = false,
                    Message = "Connection timeout, make sure that Streamlabs OBS is running and the given IP is correct."
                };
            }
            catch (IOException)
            {
                return new ConnectionStatus
                {
                    Status = false,
                    Message = "Invalid IP given. Please use a valid IP address, like 127.0.0.1 for local machine."
                };
            }
            catch (Exception e)
            {
                return new ConnectionStatus
                {
                    Status = false,
                    Message = e.Message
                };
            }
        }

        public void Close()
        {
            _client.Dispose();
            _client.Close();
            _reader.Dispose();
        }

        public string MakeRequest(string request)
        {
            _writer.Write(request);
            _writer.Flush();
            return _reader.ReadLine();
        }

        public async Task<string> MakeRequestAsync(string request)
        {
            await _writer.WriteAsync(request);
            await _writer.FlushAsync();
            return await _reader.ReadLineAsync();
        }
    }
}
