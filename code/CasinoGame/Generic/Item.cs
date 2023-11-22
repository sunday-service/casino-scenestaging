using Sandbox;

namespace Casino;

[GameResource("Generic Item", "item", "Base item objects with a description", Icon = "lunch_dining", IconBgColor = "#1396ad")]
public class Item : GameResource
{
	[Property]
	public string Name { get; set; }
	
	[Property]
	public string Description { get; set; }
	
	[Property]
	[ResourceType("png")]
	public string Icon { get; set; }
	
	[Property]
	public Model Worldmodel { get; set; }
	
	[Property]
	public Model Viewmodel { get; set; }
}
