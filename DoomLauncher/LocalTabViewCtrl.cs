namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class LocalTabViewCtrl : BasicTabViewCtrl
    {
        private ITagMapLookup m_tagLookup;

        public LocalTabViewCtrl(object key, string title, IGameFileDataSourceAdapter adapter, GameFileFieldType[] selectFields, ITagMapLookup lookup) : base(key, title, adapter, selectFields)
        {
            base.GameFileViewControl.DoomLauncherParent = this;
            this.m_tagLookup = lookup;
            if (this.m_tagLookup != null)
            {
                base.GameFileViewControl.CustomRowColorPaint = true;
                base.GameFileViewControl.CustomRowPaint += new CancelEventHandler(this.GameFileViewControl_CustomRowPaint);
            }
        }

        private void GameFileViewControl_CustomRowPaint(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            IGameFileDataSource gameFile = base.FromDataBoundItem(base.GameFileViewControl.CustomRowPaintDataBoundItem);
            if (gameFile != null)
            {
                ITagDataSource source2 = (from item in this.m_tagLookup.GetTags(gameFile)
                    where item.HasColor && item.Color.HasValue
                    select item).FirstOrDefault<ITagDataSource>();
                if (source2 != null)
                {
                    base.GameFileViewControl.CustomRowPaintForeColor = Color.FromArgb(source2.Color.Value);
                }
                else
                {
                    base.GameFileViewControl.CustomRowPaintForeColor = Control.DefaultForeColor;
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly LocalTabViewCtrl.<>c <>9 = new LocalTabViewCtrl.<>c();
            public static Func<ITagDataSource, bool> <>9__2_0;

            internal bool <GameFileViewControl_CustomRowPaint>b__2_0(ITagDataSource item) => 
                (item.HasColor && item.Color.HasValue);
        }
    }
}

