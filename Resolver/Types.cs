using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.V14;

namespace Resolver
{
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class AccountId32 : BaseType
    {
        private Arr32U8 _value;

        public Arr32U8 Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public override string TypeName()
        {
            return "AccountId32";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Value.Encode());
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Value = new Arr32U8();
            Value.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }

    [SubstrateNodeType(TypeDefEnum.Array)]
    public sealed class Arr32U8 : BaseType
    {
        private Substrate.NetApi.Model.Types.Primitive.U8[] _value;

        public override int TypeSize
        {
            get
            {
                return 32;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U8[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public override string TypeName()
        {
            return string.Format("[{0}; {1}]", new Substrate.NetApi.Model.Types.Primitive.U8().TypeName(), this.TypeSize);
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value) { result.AddRange(v.Encode()); };
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var array = new Substrate.NetApi.Model.Types.Primitive.U8[TypeSize];
            for (var i = 0; i < array.Length; i++) { var t = new Substrate.NetApi.Model.Types.Primitive.U8(); t.Decode(byteArray, ref p); array[i] = t; };
            var bytesLength = p - start;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
            Value = array;
        }

        public void Create(Substrate.NetApi.Model.Types.Primitive.U8[] array)
        {
            Value = array;
            Bytes = Encode();
        }
    }
}

