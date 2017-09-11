using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;


namespace MyGIS
{
    public partial class Buffer : Form
    {
        SaveFileDialog mSaveFileDialog;
        private IMap bufferMap;
        public string outputPathname;

        public Buffer(IMap pMap)
        {
            this.bufferMap = pMap;
            InitializeComponent();
        }

        private void Buffer_Load(object sender, EventArgs e)
        {
            //清空原有选项
            BufferLayerCombo.Items.Clear();
            //没有添加图层时返回
            if (this.bufferMap.LayerCount == 0)
            {
                MessageBox.Show("MapControl中未添加图层！", "提示");
                return;
            }
            //加载图层
            for (int i = 0; i < this.bufferMap.LayerCount; i++)
            {
                ILayer pLayer = this.bufferMap.get_Layer(i);
                BufferLayerCombo.Items.Add(pLayer.Name);
            }
            BufferLayerCombo.SelectedIndex = 0;
            BufferUnitCombo.SelectedIndex = 0;
        }

        public void CreateBuffer(IFeatureLayer pFeatureLayer)
        {
            //缓冲距离
            string bufferDistance;
            //输入的缓冲距离转换为double
            bufferDistance = txtBufferDistance.Text.ToString() + " " + BufferUnitCombo.Text.ToString();

            //判断输出路径是否合法
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(txtOutputPath.Text)) ||
              ".shp" != System.IO.Path.GetExtension(txtOutputPath.Text))
            {
                MessageBox.Show("输出路径错误!");
                return;
            }

            //获取一个geoprocessor的实例
            Geoprocessor gp = new Geoprocessor();
            //OverwriteOutput为真时，输出图层会覆盖当前文件夹下的同名图层
            gp.OverwriteOutput = true;
            //缓冲区保存路径
            string strOutputPath = txtOutputPath.Text;
            //创建一个Buffer工具的实例
            ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(pFeatureLayer, strOutputPath, bufferDistance.ToString());
            //执行缓冲区分析
            IGeoProcessorResult results = null;
            results = (IGeoProcessorResult)gp.Execute(buffer, null);
            //判断缓冲区是否成功生成
            if (results.Status != esriJobStatus.esriJobSucceeded)
                MessageBox.Show("图层" + pFeatureLayer.Name + "缓冲区生成失败！");
            else
            {
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("缓冲区生成成功！");
            }
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            mSaveFileDialog = new SaveFileDialog();
            //保存对话框的标题
            mSaveFileDialog.Title = "save Buffer shp";
            //保存对话框过滤器
            mSaveFileDialog.Filter = "shp file|*.shp";
            //图片的高度和宽度
            if (mSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = mSaveFileDialog.FileName;
            }
        }

        private void btnCreateBuffer_Click(object sender, EventArgs e)
        {
            int selectedID=BufferLayerCombo.SelectedIndex;
            IFeatureLayer pFeatureLayer=this.bufferMap.get_Layer(selectedID) as IFeatureLayer;
            outputPathname = txtOutputPath.Text;
            CreateBuffer(pFeatureLayer);
            this.Close();
        }
    }
}

