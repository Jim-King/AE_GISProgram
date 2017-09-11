using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace MyGIS
{
    public partial class MapEditForm : Form
    {
        Edit mEdit;
        private ESRI.ArcGIS.Controls.AxMapControl tempMapControl;  //mapcontrol from mainview

        public MapEditForm(ESRI.ArcGIS.Controls.AxMapControl mapcontrol)
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            tempMapControl = mapcontrol;
        }

        private void MapEditForm_Load(object sender, EventArgs e)
        {
            this.Edit_axMap.Map = tempMapControl.Map;
            //加载编辑任务
            cboTasks.SelectedIndex = 0;

            //开始编辑之前，将编辑按钮设为不可用
            this.cboTasks.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnStopEditing.Enabled = false;
        }
        /// 返回编辑状态
       
        //add layers_info to the layerselected combobox
        private void btnRenew_Click(object sender, EventArgs e)
        {
            //清空原有选项
            cboLayers.Items.Clear();
            //没有添加图层时返回
            if (this.Edit_axMap.Map.LayerCount == 0)
            {
                MessageBox.Show("MapControl中未添加图层！", "提示");
                return;
            }
            //加载图层
            for (int i = 0; i < this.Edit_axMap.Map.LayerCount; i++)
            {
                ILayer pLayer = this.Edit_axMap.get_Layer(i);
                cboLayers.Items.Add(pLayer.Name);
            }
            this.Edit_axMap.Refresh();
            cboLayers.SelectedIndex = 0;
        }

        //start editing
        private void btnStartEditing_Click(object sender, EventArgs e)
        {
            //判断是否存在可编辑图层
            if (this.Edit_axMap.Map.LayerCount == 0)
                return;
            if (this.cboLayers.Items.Count == 0)
            {
                MessageBox.Show("请选择要编辑的图层", "提示");
                return;
            }
            //获取编辑图层
            IMap pMap = this.Edit_axMap.Map;
            IFeatureLayer pFeatureLayer = this.Edit_axMap.get_Layer(cboLayers.SelectedIndex) as IFeatureLayer;
            //初始化编辑
            if (mEdit == null)
            {
                mEdit = new Edit(pFeatureLayer, pMap,this.Edit_axMap.ActiveView);
            }
            //开始编辑
            mEdit.StartEditing();
            //开始编辑设为不可用，将其他编辑按钮设为可用
            this.btnStartEditing.Enabled = false;
            this.cboTasks.Enabled = true;
            this.btnStopEditing.Enabled = true;
            this.btnSave.Enabled = true;

        }

        //save the editing
        private void btnSave_Click(object sender, EventArgs e)
        {
            //判断编辑是否初始化
            if (mEdit == null)
                return;
            //处于编辑状态且已编辑则保存
            if (mEdit.IsEditing() && mEdit.HasEdited())
            {
                mEdit.SaveEditing(true);
            }
            this.Edit_axMap.Refresh();

        }

        //stop editing and save editing
        private void btnStopEditing_Click(object sender, EventArgs e)
        {
            if (mEdit == null)
                return;
            if (mEdit.HasEdited())
            {
                DialogResult dr = MessageBox.Show("图层已编辑，是否保存？", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                    mEdit.SaveEditing(true);
                else
                    mEdit.SaveEditing(false);
            }
            this.cboTasks.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnStopEditing.Enabled = false;
            this.btnStartEditing.Enabled = true;

        }

        private void Edit_axMap_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            //判断是否鼠标左键
            if (e.button != 1)
                return;
            //判断是否处于编辑状态
            if (mEdit.IsEditing())
            {
                switch (cboTasks.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        mEdit.PanMouseUp(e.mapX, e.mapY);
                        break;
                }
            }

        }

        private void Edit_axMap_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //判断是否鼠标左键
            if (e.button != 1)
                return;
            //判断是否处于编辑状态
            if (mEdit.IsEditing())
            {
                switch (cboTasks.SelectedIndex)
                {
                    case 0:
                        mEdit.CreateMouseDown(e.mapX, e.mapY);
                        break;
                    case 1:
                        mEdit.PanMouseDown(e.mapX, e.mapY);
                        break;
                }
            }

        }

        private void Edit_axMap_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //判断是否处于编辑状态
            if (mEdit.IsEditing())
            {
                switch (cboTasks.SelectedIndex)
                {
                    case 0:
                    case 1:
                        mEdit.pMouseMove(e.mapX, e.mapY);
                        break;
                }
            }

        }

        private void Edit_axMap_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            //判断是否鼠标左键
            if (e.button != 1)
                return;

            //判断是否处于编辑状态
            if (mEdit.IsEditing())
            {
                switch (cboTasks.SelectedIndex)
                {
                    case 0:
                        mEdit.CreateDoubleClick(e.mapX, e.mapY);
                        break;
                    case 1:
                        break;
                }
            }

        }


    }
}
