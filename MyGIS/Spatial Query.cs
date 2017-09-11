﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

using System.Windows.Forms;

namespace MyGIS
{
    public partial class Spatial_Query : Form
    {
        //获取主界面的MapControl对象
        private AxMapControl mMapControl;
        //查询方式
        public int mQueryMode;
        //图层索引
        public int mLayerIndex;

        public Spatial_Query (AxMapControl mapControl)
        {
            InitializeComponent();
            this.mMapControl = mapControl;
        }


        private void Spatial_Query_Load(object sender, EventArgs e)
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
                //图层名称加入ComboBox
                this.spaQueryLayer.Items.Add(strLayerName);
            }

            //加载查询方式
            this.spaQueryType.Items.Add("矩形查询");
            this.spaQueryType.Items.Add("线查询");
            this.spaQueryType.Items.Add("点查询");
            this.spaQueryType.Items.Add("圆查询");

            //初始化ComboBox默认值
            this.spaQueryLayer.SelectedIndex = 0;
            this.spaQueryType.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //设置鼠标点击时窗体的结果
            this.DialogResult = DialogResult.OK;
            //判断是否存在图层
            if (this.spaQueryLayer.Items.Count <= 0)
            {
                MessageBox.Show("当前MapControl没有添加图层！", "提示");
                return;
            }
            //获取选中的查询方式和图层索引
            this.mLayerIndex = this.spaQueryLayer.SelectedIndex;
            this.mQueryMode = this.spaQueryType.SelectedIndex;
        }
    }
}
