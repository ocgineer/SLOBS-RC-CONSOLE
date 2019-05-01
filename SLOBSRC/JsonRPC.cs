using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SLOBSRC
{
    /// <summary>
    /// JSON-RPC Message base
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// JSON-RPC version number.
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string Version { get; internal protected set; }

        /// <summary>
        /// JSON-RPC request identifactor.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; internal protected set; }

        /// <summary>
        /// Serializes this JSON-RPC message object to json string.
        /// </summary>
        /// <returns>Json string.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// JSON-RPC Request Message object
    /// </summary>
    /// <typeparam name="T">Type of the expected Response</typeparam>
    public sealed class Request<T> : Message
    {
        /// <summary>
        /// Name of the method to be invoked.
        /// </summary>
        [JsonProperty("method")]
        public string Method;

        /// <summary>
        /// A Structured value that holds the parameter values to be used during the invocation of the method.
        /// </summary>
        [JsonProperty("params")]
        public object Params;

        /// <summary>
        /// Creates a new JSON-RPC Request message object.
        /// </summary>
        public Request()
        {
            base.Version = "2.0";
            base.Id = GetHashCode();
        }

        /// <summary>
        /// Creates a new JSON-RPC Request message object.
        /// </summary>
        /// <param name="method">Name of the method to be invoked.</param>
        /// <param name="parameters">Structured value that holds the parameter values to be used during the invocation of the method.</param>
        public Request(string method, object parameters)
        {
            base.Version = "2.0";
            base.Id = GetHashCode();
            this.Method = method;
            this.Params = parameters;
        }

        #region Streamlabs OBS JSON-RPC Specific

        private sealed class SlobsParams
        {
            [JsonProperty("resource")]
            public string Resource { get; internal set; }

            [JsonProperty("args")]
            public object[] Args { get; internal set; }
        }

        /// <summary>
        /// Creates a new JSON-RPC Request message object with resource and arguments for Streamlabs OBS
        /// </summary>
        /// <param name="method">Name of the method to be invoked.</param>
        /// <param name="resource">Resource of Streamlabs OBS object JSON-RPC.</param>
        /// <param name="args">Additional arguments for Streamlabs OBS JSON-RPC.</param>
        public Request(string method, string resource, object[] args = null)
        {
            base.Version = "2.0";
            base.Id = GetHashCode();
            this.Method = method;
            this.Params = new SlobsParams()
            {
                Resource = resource,
                Args = args
            };
        }

        #endregion


        /// <summary>
        /// Serializes and sends this JSON-RPC Message object to connected JSON-RPC Server.
        /// </summary>
        /// <param name="connection">Active connection to the JSON-RPC server.</param>
        /// <returns>Expected Type model of the request.</returns>
        public Response<T> GetResponse(IConnection connection)
        {
            var response = connection.MakeRequest(this.ToString());
            return JsonConvert.DeserializeObject<Response<T>>(response);
        }

        /// <summary>
        /// Serializes and sends this JSON-RPC Message object to connected JSON-RPC Server async.
        /// </summary>
        /// <param name="connection">Active connection to the JSON-RPC server.</param>
        /// <returns>Expected Type model of the request.</returns>
        public async Task<Response<T>> GetResponseAsync(IConnection connection)
        {
            var response = await connection.MakeRequestAsync(this.ToString());
            return JsonConvert.DeserializeObject<Response<T>>(response);
        }
    }

    /// <summary>
    /// JSON-RPC Response Message object
    /// </summary>
    /// <typeparam name="T">Type of the expected Response</typeparam>
    public sealed class Response<T> : Message
    {
        public sealed class ErrorObject
        {
            /// <summary>
            /// A Number that indicates the error type that occurred.
            /// </summary>
            [JsonProperty("code")]
            public int Code { get; private set; }

            /// <summary>
            /// A String providing a short description of the error.
            /// </summary>
            [JsonProperty("message")]
            public string Message { get; private set; }

            /// <summary>
            /// A Primitive or Structured value that contains additional information about the error.
            /// </summary>
            [JsonProperty("data")]
            public object Data { get; private set; }
        }

        /// <summary>
        /// JSON-RPC call error.
        /// </summary>
        [JsonProperty("error")]
        public ErrorObject Error { get; private set; }

        /// <summary>
        /// JSON-RPC call result.
        /// </summary>
        [JsonProperty("result")]
        public T Result { get; private set; }

        /// <summary>
        /// Returns received expected Type.
        /// </summary>
        /// <returns>Expected Type.</returns>
        public T GetResult()
        {
            return this.Result;
        }

        /// <summary>
        /// Indication of a successfull request. True if the Error field is null, false otherwise.
        /// </summary>
        public bool Success
        {
            get
            {
                return this.Error == null;
            }
        }
    }
}
