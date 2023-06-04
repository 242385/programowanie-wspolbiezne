using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.Serialization;

namespace Dane
{
    public abstract class IBall : IObservable<IBall>, ISerializable, IDisposable
    {
        public abstract int BallID { get; }
        [JsonConverter(typeof(Vector2Converter))]
        public abstract Vector2 Coordinates { get; set; }
        [JsonIgnore]
        public abstract Vector2 VelVector { get; set;}
        [JsonIgnore]
        public abstract float DeltaTime { get; set; }
        [JsonIgnore]
        public abstract float Mass { get; set; }
        [JsonIgnore]
        public abstract bool StopTask { get; set; }
        [JsonIgnore]
        public abstract float Radius { get; set; }
        [JsonIgnore]
        public abstract bool StartMoving { get; set; }
        [JsonIgnore]
        public abstract bool IsInACollision { get; set; }
        public abstract void Dispose();
        public static IBall CreateBall(int ballID, float mass, float radius, Vector2 coords, Vector2 vector, float delta, ILogger? logger)
        {
            return new Ball(ballID, mass, radius, coords, vector, delta, logger);
        }
     
        public abstract IDisposable Subscribe(IObserver<IBall> observerObj);

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
    internal class Vector2Converter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteEndObject();
        }
    }
 }
