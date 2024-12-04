using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Shops;
using SObject = StardewValley.Object;

namespace DoomValley;

public class DoomValleyMod : Mod
{
    private DoomMinigame? _minigame;
    private Texture2D? _textureOverlay;

    public override void Entry(IModHelper helper)
    {
        _textureOverlay = helper.ModContent.Load<Texture2D>("assets/doom_arcade_machine.png");
        _minigame = new DoomMinigame(helper.DirectoryPath + "/");
        helper.Events.Content.AssetRequested += OnAssetRequested;
        helper.Events.Input.ButtonPressed += OnButtonPressed;
        helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
    }

    private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        if (e.Name.IsEquivalentTo("Data/BigCraftables"))
            e.Edit(asset =>
            {
                var data = asset.AsDictionary<string, BigCraftableData>().Data;
                data["666"] = new BigCraftableData
                {
                    Name = "Doom Arcade System",
                    DisplayName = "Doom Arcade System",
                    Description = "Yes, it can run doom.",
                    Price = 5000,
                    Fragility = SObject.fragility_Removable,
                    CanBePlacedOutdoors = true,
                    CanBePlacedIndoors = true,
                    IsLamp = false,
                    SpriteIndex = 299
                };
            });
        else if (e.Name.IsEquivalentTo("Data/Shops"))
            e.Edit(asset =>
            {
                var data = asset.AsDictionary<string, ShopData>().Data;
                data["Saloon"].Items.Insert(0, new ShopItemData
                {
                    Id = "DoomValley.666_DoomArcadeSystem",
                    ItemId = "666"
                });
            });
        else if (e.Name.IsEquivalentTo("TileSheets/Craftables"))
            e.Edit(asset =>
            {
                var data = asset.AsImage().Data;
                var sourcePixels = new Color[_textureOverlay!.Width * _textureOverlay.Height];
                _textureOverlay.GetData(sourcePixels);
                data.SetData(0, new Rectangle(48, 1184, _textureOverlay.Width, _textureOverlay.Height), sourcePixels,
                    0, sourcePixels.Length);
            });
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsWorldReady || !e.Button.IsActionButton()) return;

        var tile = e.Cursor.Tile;
        var obj = Game1.currentLocation.getObjectAtTile((int)tile.X, (int)tile.Y);

        if (obj is { QualifiedItemId: "(BC)666" } &&
            Utility.tileWithinRadiusOfPlayer((int)tile.X, (int)tile.Y, 1, Game1.player))
            Game1.currentMinigame = _minigame;
    }

    private static void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady) return;

        var tile = Game1.currentCursorTile;
        var obj = Game1.currentLocation.getObjectAtTile((int)tile.X, (int)tile.Y);

        if (obj is not { QualifiedItemId: "(BC)666" }) return;
        if (!Utility.tileWithinRadiusOfPlayer((int)tile.X, (int)tile.Y, 1, Game1.player))
            Game1.mouseCursorTransparency = 0.5f;

        Game1.mouseCursor = Game1.cursor_grab;
    }
}