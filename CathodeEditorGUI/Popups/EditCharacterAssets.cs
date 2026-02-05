using AlienPAK;
using CATHODE;
using CATHODE.Enums;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CATHODE.CustomCharacterAssetData;

namespace CommandsEditor.Popups
{
    public partial class EditCharacterAssets : BaseWindow
    {
        CustomCharacterAssetData _assetData;
        CustomCharacterAssetData.AssetDefinition _assetDefinition = null;

        public EditCharacterAssets() : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _assetData = new CustomCharacterAssetData(SharedData.pathToAI + "/DATA/CHR_INFO/CUSTOMCHARACTERASSETDATA.BIN");

            assetSetList.BeginUpdate();
            assetSetList.Items.Clear();
            foreach (CUSTOM_CHARACTER_ASSETS assetSet in Enum.GetValues(typeof(CUSTOM_CHARACTER_ASSETS)))
            {
                assetSetList.Items.Add(assetSet.ToString());
            }
            assetSetList.Items.RemoveAt(assetSetList.Items.Count - 1);
            assetSetList.EndUpdate();

            this.Load += EditCharacterAssets_Load;
        }

        private void EditCharacterAssets_Load(object sender, EventArgs e)
        {
            assetSetList.SelectedIndex = 0;
        }

        private void assetSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _assetDefinition = _assetData.Entries[assetSetList.SelectedIndex];
            ReloadUI();
        }

        private void ReloadUI()
        {
            SetColourPreview(ColourType.PRIMARY, primaryColourList);
            SetColourPreview(ColourType.SECONDARY, secondaryColourList);
            SetColourPreview(ColourType.TERTIARY, tertiaryColourList);

            decalList.BeginUpdate();
            decalList.Items.Clear();
            decalImageList.Images.Clear();
            int iconSize = Math.Max(16, decalImageList.ImageSize.Width);
            foreach (string decal in _assetDefinition.Decals)
            {
                Bitmap thumb = CreateDecalThumbnail(decal, iconSize);
                try
                {
                    decalImageList.Images.Add(thumb);
                    decalList.Items.Add(new ListViewItem(decal, decalImageList.Images.Count - 1));
                }
                finally
                {
                    thumb?.Dispose();
                }
            }
            decalList.EndUpdate();
        }

        private Bitmap CreateDecalThumbnail(string decal, int iconSize)
        {
            Bitmap thumb = new Bitmap(iconSize, iconSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Bitmap texture = Content.Level.Textures.Entries.FirstOrDefault(o => o.Name.ToUpper() == decal.ToUpper())?.ToBitmap();
            if (texture != null && texture.Width > 0 && texture.Height > 0)
            {
                try
                {
                    using (var g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(texture, 0, 0, iconSize, iconSize);
                    }
                }
                finally
                {
                    texture.Dispose();
                }
            }
            else
            {
                texture?.Dispose();
                using (var g = Graphics.FromImage(thumb))
                    g.Clear(Color.LightGray);
            }
            return thumb;
        }

        private void SetColourPreview(ColourType colourType, ListView ui)
        {
            ui.BeginUpdate();
            ui.Items.Clear();
            List<Vector3> colours = _assetDefinition.Tints[colourType];
            foreach (Vector3 colour in colours)
            {
                //todo - need to make big square
                ui.Items.Add(new ListViewItem(" ") { BackColor = Color.FromArgb(255, (int)(colours[0].X * 255.0f), (int)(colours[0].Y * 255.0f), (int)(colours[0].Z * 255.0f)) });
            }
            ui.EndUpdate();
        }

        private void addNewPrimary_Click(object sender, EventArgs e)
        {

        }

        private void removeSelectedPrimary_Click(object sender, EventArgs e)
        {

        }

        private void addNewSecondary_Click(object sender, EventArgs e)
        {

        }

        private void removeSelectedSecondary_Click(object sender, EventArgs e)
        {

        }

        private void addNewTertiary_Click(object sender, EventArgs e)
        {

        }

        private void removeSelectedTertiary_Click(object sender, EventArgs e)
        {

        }

        private void addNewDecal_Click(object sender, EventArgs e)
        {

        }

        private void removeSelectedDecal_Click(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            _assetData.Save();
            this.Close();
        }
    }
}
