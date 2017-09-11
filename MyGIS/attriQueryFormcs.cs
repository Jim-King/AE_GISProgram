using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using System.Windows.Forms;

namespace MyGIS
{
    public partial class attriQueryFormcs : Form
    {
        //地图数据
        private AxMapControl mMapControl;
        //选中图层
        public IFeatureLayer mFeatureLayer;
        public IFeature pFeature;

        public attriQueryFormcs(AxMapControl mapcontrol)
        {
            InitializeComponent();
            this.mMapControl = mapcontrol;
        }

        private void attriQueryFormcs_Load(object sender, EventArgs e)
        {
            //MapControl中没有图层时返回
            if (this.mMapControl.LayerCount <= 0)
                return;

            //获取MapControl中的全部图层名称，并加入ComboBox
            //图层
            ILayer pLayer;
            //图层名称
            string strLayerName;
            for (int i = 0; i < this.mMapControl.LayerCount; i++)
            {
                pLayer = this.mMapControl.get_Layer(i);
                strLayerName = pLayer.Name;
                //图层名称加入cboLayer
                this.comboLayer.Items.Add(strLayerName);
            }
            //默认显示第一个选项
            this.comboLayer.SelectedIndex = 0;

        }

        private void comboLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboField.Items.Clear();
            //获取cboLayer中选中的图层
            mFeatureLayer = mMapControl.get_Layer(comboLayer.SelectedIndex) as IFeatureLayer;
            IFeatureClass pFeatureClass = mFeatureLayer.FeatureClass;
            //字段名称
            string strFldName;
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                strFldName = pFeatureClass.Fields.get_Field(i).Name;
                //图层名称加入cboField
                this.comboField.Items.Add(strFldName);
            }
            //默认显示第一个选项
            this.comboField.SelectedIndex = 0;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //定义图层，要素游标，查询过滤器，要素
            IFeatureLayer pFeatureLayer=this.mFeatureLayer;
            IFeatureCursor pFeatureCursor;
            IQueryFilter pQueryFilter;
            string FieldName;

            //pQueryFilter的实例化
            pQueryFilter = new QueryFilterClass();
            //设置查询过滤条件
            FieldName = comboField.SelectedItem.ToString();
            pQueryFilter.WhereClause = FieldName+"='" + txtValue.Text + "'";
            //查询
            pFeatureCursor = pFeatureLayer.Search(pQueryFilter, true);
            //获取查询到的要素
            pFeature = pFeatureCursor.NextFeature();

            //判断是否获取到要素
            if (pFeature != null)
            {
                this.DialogResult = DialogResult.OK;
                //选择要素
            }
            else
            {
                //没有得到pFeature的提示
                MessageBox.Show("没有找到名为" + txtValue.Text + "的州", "提示");
            }

        }
    }
}
