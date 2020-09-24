namespace dotnet_fmstsngn.Models
{
	public class Header<key, value>
	{
		public key id { get; set; }
		public value text { get; set; }

		public Header() { }
		public Header(key key, value val)
		{
			this.id = key;
			this.text = val;
		}
	}
}