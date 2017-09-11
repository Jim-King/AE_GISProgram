using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using MyCalculator;

namespace MyGIS
{
    public partial class Form1 : Form
    {
        OpenFileDialog mOpenFileDialog;
        SaveFileDialog mSaveFileDialog;
        IHit3DSet mHit3DSet;
        Scene_Point_Query mResultForm;
        MapEditForm newEditForm;
        Buffer pBuffer;
        Spatial_Query spatialQueryForm;
        attriQueryFormcs attributeQForm;
        RoadNet_System networkForm;
        //空间查询的查询方式
        private int mQueryMode;
        //图层索引
        private int mLayerIndex;
        private string mTool;

    

        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            
        }
        // activate calculator
        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCalculator.Form1 myform = new MyCalculator.Form1();
            myform.Show();
        }
        //initiate Form
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //open a sxd File
        private void OpenSxdFile_Click(object sender, EventArgs e)
        {
           mOpenFileDialog= new OpenFileDialog();
            //文件过滤
            mOpenFileDialog.Filter = "sxd文件|*.sxd";
            //打开文件对话框打开事件
            if (mOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //从打开对话框中得到打开文件的全路径,并将该路径传入到axSceneControl1中
                axSceneControl1.LoadSxFile(mOpenFileDialog.FileName);
            }

        }
        //open a raster File
        private void OpenRasterFile_Click(object sender, EventArgs e)
        {
            mOpenFileDialog = new OpenFileDialog();
            string sFileName = null;
            //新建栅格图层
            IRasterLayer pRasterLayer = null;
            pRasterLayer = new ESRI.ArcGIS.Carto.RasterLayerClass();
            //取消文件过滤
            mOpenFileDialog.Filter = "所有文件|*.*";
            //打开文件对话框打开事件
            if (mOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //从打开对话框中得到打开文件的全路径
                sFileName = mOpenFileDialog.FileName;
                //创建栅格图层
                pRasterLayer.CreateFromFilePath(sFileName);
                //将图层加入到控件中
                axSceneControl1.Scene.AddLayer(pRasterLayer, true);
                //将当前视点跳转到栅格图层
                ICamera pCamera = axSceneControl1.Scene.SceneGraph.ActiveViewer.Camera;
                //得到范围
                IEnvelope pEenvelop = pRasterLayer.VisibleExtent;
                //添加z轴上的范围
                pEenvelop.ZMin = axSceneControl1.Scene.Extent.ZMin;
                pEenvelop.ZMax = axSceneControl1.Scene.Extent.ZMax;
                //设置相机
                pCamera.SetDefaultsMBB(pEenvelop);
                axSceneControl1.Refresh();
            }

        }
        //save the sxd to image
        private void SaveImage_Click(object sender, EventArgs e)
        {
            string sFileName = "";
            mSaveFileDialog = new SaveFileDialog();
            //保存对话框的标题
            mSaveFileDialog.Title = "保存图片";
            //保存对话框过滤器
            mSaveFileDialog.Filter = "BMP图片|*.bmp|JPG图片|*.jpg";
            //图片的高度和宽度
            int Width = axSceneControl1.Width;
            int Height =axSceneControl1.Height;
            if (mSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                sFileName = mSaveFileDialog.FileName;
                if (mSaveFileDialog.FilterIndex == 1)//保存成BMP格式的文件
                {
                    axSceneControl1.SceneViewer.GetSnapshot(Width, Height,
                        esri3DOutputImageType.BMP, sFileName);
                }
                else//保存成JPG格式的文件
                {
                    axSceneControl1.SceneViewer.GetSnapshot(Width, Height,
                        esri3DOutputImageType.JPEG, sFileName);
                }
                MessageBox.Show("保存图片成功！");
                axSceneControl1.Refresh();
            }

        }
        //point_query on the sxd File
        private void axSceneControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseDownEvent e)
        {
            if (Point_Query.Checked)//check按钮处于打勾状态
            {
                //查询
                mHit3DSet = new Hit3DSet();
                axSceneControl1.SceneGraph.LocateMultiple(axSceneControl1.SceneGraph.ActiveViewer,
                      e.x, e.y, esriScenePickMode.esriScenePickAll, false, out mHit3DSet);
                mHit3DSet.OnePerLayer();
                if (mHit3DSet == null)//没有选中对象
                {
                    MessageBox.Show("没有选中对象");
                }
                else
                {
                    //显示在ResultForm控件中。mHit3DSet为查询结果集合
                    mResultForm = new Scene_Point_Query();
                    mResultForm.Show();
                    mResultForm.refreshView(mHit3DSet);
                }
                axSceneControl1.Refresh();
            }

        }
        //get layer_info of all layers in sxd
        private void refresh_layer_btn_Click(object sender, EventArgs e)
        {
            mLayerComBox.Items.Clear();
            //得到当前场景中所有图层
            int nCount = axSceneControl1.Scene.LayerCount;
            if (nCount <= 0)//没有图层的情况
            {
                MessageBox.Show("场景中没有图层，请加入图层");
                return;
            }
            int i;
            ILayer pLayer = null;
            //将所有的图层的名称显示到复选框中
            for (i = 0; i < nCount; i++)
            {
                pLayer = axSceneControl1.Scene.get_Layer(i);
                mLayerComBox.Items.Add(pLayer.Name);
            }
            //将复选框设置为选中第一项
            mLayerComBox.SelectedIndex = 0;
            addFieldNameToCombox(mLayerComBox.Items[mLayerComBox.SelectedIndex].ToString());

        }
        //get fields of one particular layer in sxd as you choose
        private void addFieldNameToCombox(string layerName)
        {
            mFieldComBox.Items.Clear();
            int i;
            IFeatureLayer pFeatureLayer = null;
            IFields pField = null;
            int nCount = axSceneControl1.Scene.LayerCount;
            ILayer pLayer = null;
            //寻找名称为layerName的FeatureLayer;
            for (i = 0; i < nCount; i++)
            {
                pLayer = axSceneControl1.Scene.get_Layer(i) as IFeatureLayer;
                if (pLayer.Name == layerName)//找到了layerName的Featurelayer
                {
                    pFeatureLayer = pLayer as IFeatureLayer;
                    break;
                }
            }
            if (pFeatureLayer != null)//判断是否找到
            {
                pField = pFeatureLayer.FeatureClass.Fields;
                nCount = pField.FieldCount;
                //将该图层中所用的字段写入到mFeildCombox中去
                for (i = 0; i < nCount; i++)
                {
                    mFieldComBox.Items.Add(pField.get_Field(i).Name);
                }
            }
            mFieldComBox.SelectedIndex = 0;
        }
        //change the shown fields in combobox when change the choosed layer
        private void mLayerComBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            addFieldNameToCombox(mLayerComBox.Items[mLayerComBox.SelectedIndex].ToString());
        }
        //create TIN with  the field in the layer and particualr solution
        private void create_TIN_btn_Click(object sender, EventArgs e)
        {
            if (mLayerComBox.Text == "" || mFieldComBox.Text == "")//判断输入合法性
            {
                MessageBox.Show("没有相应的图层");
                return;
            }
            ITinEdit pTin = new TinClass();
            //寻找Featurelayer
            IFeatureLayer pFeatureLayer =
                axSceneControl1.Scene.get_Layer(mLayerComBox.SelectedIndex) as IFeatureLayer;
            if (pFeatureLayer != null)
            {
                IEnvelope pEnvelope = new EnvelopeClass();
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                IField pField = null;
                //找字段
                pField = pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindField(mFieldComBox.Text));
                if (pField.Type == esriFieldType.esriFieldTypeInteger ||
                     pField.Type == esriFieldType.esriFieldTypeDouble ||
                     pField.Type == esriFieldType.esriFieldTypeSingle)//判断类型
                {
                    IGeoDataset pGeoDataset = pFeatureLayer as IGeoDataset;
                    pEnvelope = pGeoDataset.Extent;
                    //设置空间参考系
                    ISpatialReference pSpatialReference;
                    pSpatialReference = pGeoDataset.SpatialReference;
                    //选择生成TIN的输入类型
                    esriTinSurfaceType pSurfaceTypeCount = esriTinSurfaceType.esriTinMassPoint;
                    switch (mTypeComBox.Text)
                    {
                        case "POINT":
                            pSurfaceTypeCount = esriTinSurfaceType.esriTinMassPoint;
                            break;
                        case "POLYLINE":
                            pSurfaceTypeCount = esriTinSurfaceType.esriTinSoftLine;
                            break;
                        case "CURVE":
                            pSurfaceTypeCount = esriTinSurfaceType.esriTinHardLine;
                            break;
                    }
                    //创建TIN
                    pTin.InitNew(pEnvelope);
                    object missing = Type.Missing;
                    //生成TIN
                    pTin.AddFromFeatureClass(pFeatureClass, pQueryFilter, pField, pField, pSurfaceTypeCount, ref missing);
                    pTin.SetSpatialReference(pGeoDataset.SpatialReference);
                    //创建Tin图层并将Tin图层加入到场景中去
                    ITinLayer pTinLayer = new TinLayerClass();
                    pTinLayer.Dataset = pTin as ITin;
                    axSceneControl1.Scene.AddLayer(pTinLayer,true);
                }
                else
                {
                    MessageBox.Show("该字段的类型不符合构建TIN的条件");
                }
            }

        }
        //hulk eye function
        private void mainView_axMap_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //创建鹰眼中线框
            IEnvelope pEnv = (IEnvelope)e.newEnvelope;
            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pEle = pRectangleEle as IElement;
            pEle.Geometry = pEnv;

            //设置线框的边线对象，包括颜色和线宽
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 0;
            pColor.Green = 255;
            pColor.Blue = 0;
            pColor.Transparency = 200;
            // 产生一个线符号对象 
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 2;
            pOutline.Color = pColor;
            //change the color for content
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 0;

            // 设置线框填充符号的属性 
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;

            // 得到鹰眼视图中的图形元素容器
            IGraphicsContainer pGra = HulkEye_axMap.Map as IGraphicsContainer;
            IActiveView pAv = pGra as IActiveView;
            // 在绘制前，清除 axMapControl2 中的任何图形元素 
            pGra.DeleteAllElements();
            // 鹰眼视图中添加线框
            pGra.AddElement((IElement)pFillShapeEle, 0);
            // 刷新鹰眼
            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);    

        }
        //hulk eye function
        private ILayer GetOverviewLayer(IMap map)
        {
            //获取主视图的第一个图层
            ILayer pLayer = map.get_Layer(0);
            //遍历其他图层，并比较视图范围的宽度，返回宽度最大的图层
            ILayer pTempLayer = null;
            for (int i = 1; i < map.LayerCount;i++ )
            {
                pTempLayer = map.get_Layer(i);
                if (pLayer.AreaOfInterest.Width < pTempLayer.AreaOfInterest.Width)
                    pLayer = pTempLayer;
            }
            return pLayer;
        }
        //hulk eye function
        private void mainView_axMap_OnMapReplaced(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMapReplacedEvent e)
          {
              //获取鹰眼图层
              this.HulkEye_axMap.AddLayer(this.GetOverviewLayer(this.mainView_axMap.Map));
              // 设置 MapControl 显示范围至数据的全局范围
              this.HulkEye_axMap.Extent = this.mainView_axMap.FullExtent;
              // 刷新鹰眼控件地图
              this.HulkEye_axMap.Refresh();

          }
        //hulk eye function
        private void HulkEye_axMap_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
          {
              // 如果不是左键按下就直接返回 
              if (e.button != 1) return;
              IPoint pPoint = new PointClass();
              pPoint.PutCoords(e.mapX, e.mapY);
              this.mainView_axMap.CenterAt(pPoint);
              this.mainView_axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

          }
        //hulk eye function
        private void HulkEye_axMap_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
          {
              if (this.HulkEye_axMap.Map.LayerCount != 0)
              {
                  // 按下鼠标左键移动矩形框 
                  if (e.button == 1)
                  {
                      IPoint pPoint = new PointClass();
                      pPoint.PutCoords(e.mapX, e.mapY);
                      IEnvelope pEnvelope = this.mainView_axMap.Extent;
                      pEnvelope.CenterAt(pPoint);
                      this.mainView_axMap.Extent = pEnvelope;
                      this.mainView_axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                  }
                  // 按下鼠标右键绘制矩形框 
                  else if (e.button == 2)
                  {
                     
                      IEnvelope pEnvelop =this.HulkEye_axMap.TrackRectangle() ;
                      this.mainView_axMap.Extent = pEnvelop;
                      this.mainView_axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                  }
              }

          }
        //boon
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You Fucking idiot!");
        }
        //start editing map layers
        private void mapEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newEditForm = new MapEditForm(this.mainView_axMap);
            newEditForm.Show();
            
        }
        //create buffer form
        private void bufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBuffer = new Buffer(this.mainView_axMap.Map);
            if (pBuffer.ShowDialog() == DialogResult.OK)
            {
                //获取输出文件路径
                string strBufferPath = pBuffer.outputPathname;
                //缓冲区图层载入到MapControl
                int index = strBufferPath.LastIndexOf("\\");
                this.mainView_axMap.AddShapeFile(strBufferPath.Substring(0, index), strBufferPath.Substring(index));
            }

        }
        //carry out spatial query
        private void spatialQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spatialQueryForm = new Spatial_Query(this.mainView_axMap);
            if (spatialQueryForm.ShowDialog() == DialogResult.OK)
            {
                //标记为“空间查询”
                this.mTool = "SpaceQuery";
                //获取查询方式和图层
                this.mQueryMode = spatialQueryForm.mQueryMode;
                this.mLayerIndex = spatialQueryForm.mLayerIndex;
                //定义鼠标形状
                this.mainView_axMap.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerCrosshair;
            }

        }
        //refresh coordination and scales
        private void mainView_axMap_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            // 显示当前比例尺
            this.toolStripStatusLabel1.Text = " 比例尺 1:" + ((long)this.mainView_axMap.MapScale).ToString();
            // 显示当前坐标
            this.toolStripStatusLabel2.Text = " 当前坐标 X = " + e.mapX.ToString() + " Y = " + e.mapY.ToString() + " " + this.mainView_axMap.MapUnits;

        }
        //convert data from Layer to DataGrid in spatial query
        private DataTable LoadQueryResult(AxMapControl mapControl, IFeatureLayer featureLayer, IGeometry geometry)
        {
            IFeatureClass pFeatureClass = featureLayer.FeatureClass;

            //根据图层属性字段初始化DataTable
            IFields pFields = pFeatureClass.Fields;
            DataTable pDataTable = new DataTable();
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string strFldName;
                strFldName = pFields.get_Field(i).AliasName;
                pDataTable.Columns.Add(strFldName);
            }

            //空间过滤器
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = geometry;

            //根据图层类型选择缓冲方式
            switch (pFeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
            }
            //定义空间过滤器的空间字段
            pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;

            IQueryFilter pQueryFilter;
            IFeatureCursor pFeatureCursor;
            IFeature pFeature;
            //利用要素过滤器查询要素
            pQueryFilter = pSpatialFilter as IQueryFilter;
            pFeatureCursor = featureLayer.Search(pQueryFilter, true);
            pFeature = pFeatureCursor.NextFeature();

            while (pFeature != null)
            {
                string strFldValue = null;
                DataRow dr = pDataTable.NewRow();
                //遍历图层属性表字段值，并加入pDataTable
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string strFldName = pFields.get_Field(i).Name;
                    if (strFldName == "Shape")
                    {
                        strFldValue = Convert.ToString(pFeature.Shape.GeometryType);
                    }
                    else
                        strFldValue = Convert.ToString(pFeature.get_Value(i));
                    dr[i] = strFldValue;
                }
                pDataTable.Rows.Add(dr);
                //高亮选择要素
                mapControl.Map.SelectFeature((ILayer)featureLayer, pFeature);
                mapControl.ActiveView.Refresh();
                pFeature = pFeatureCursor.NextFeature();
            }
            return pDataTable;
        }

        private void mainView_axMap_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            this.mainView_axMap.Map.ClearSelection();
            //获取当前视图
            IActiveView pActiveView = this.mainView_axMap.ActiveView;
            //获取鼠标点
            IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            switch (mTool)
            {
                case "SpaceQuery":
                    IGeometry pGeometry = null;
                    if (this.mQueryMode == 0)//矩形查询
                    {
                        pGeometry = this.mainView_axMap.TrackRectangle();
                    }
                    else if (this.mQueryMode == 1)//线查询
                    {
                        pGeometry = this.mainView_axMap.TrackLine();
                    }
                    else if (this.mQueryMode == 2)//点查询
                    {
                        ITopologicalOperator pTopo;
                        IGeometry pBuffer;
                        pGeometry = pPoint;
                        pTopo = pGeometry as ITopologicalOperator;
                        //根据点位创建缓冲区，缓冲半径为0.1，可修改
                        pBuffer = pTopo.Buffer(0.1);
                        pGeometry = pBuffer.Envelope;
                    }
                    else if (this.mQueryMode == 3)//圆查询
                    {
                        pGeometry = this.mainView_axMap.TrackCircle();
                    }
                    IFeatureLayer pFeatureLayer = this.mainView_axMap.get_Layer(this.mLayerIndex) as IFeatureLayer;
                    DataTable pDataTable = this.LoadQueryResult(this.mainView_axMap, pFeatureLayer, pGeometry);
                    this.dataGridView1.DataSource = pDataTable.DefaultView;
                    this.dataGridView1.Refresh();
                    chart1.Series.Clear();
                    Series Test = new Series("TestData");
                    Test.ChartType = SeriesChartType.Bar;
                    Test.AxisLabel = "STATE NAME";
                    for (int i=0;i<pDataTable.Rows.Count;i++)
                    {
                        Test.Points.AddXY(pDataTable.Rows[i]["STATE_NAME"], pDataTable.Rows[i]["AREA"]);
                    }
                    chart1.Series.Add(Test);
                    break;
                default:
                    break;
            }

        }

        private void attributeQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mainView_axMap.Map.ClearSelection();
            attributeQForm = new attriQueryFormcs(this.mainView_axMap);
            if (attributeQForm.ShowDialog()==DialogResult.OK)
            {
                this.mainView_axMap.Map.SelectFeature(attributeQForm.mFeatureLayer, attributeQForm.pFeature);
                this.mainView_axMap.Extent = attributeQForm.pFeature.Shape.Envelope;
            }
        }

        private void roadNetSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            networkForm = new RoadNet_System();
            networkForm.Show();
        }

    }
}
