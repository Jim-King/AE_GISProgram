using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;


namespace MyGIS
{
    class Edit
    {
        private bool mIsEditing;                          //编辑状态
        private bool mHasEditing;                         //是否编辑
        private IFeatureLayer mCurrentLayer;              //当前编辑图层
        private IWorkspaceEdit mWorkspaceEdit;            //编辑工作空间
        private IMap mMap;                                //地图
        private IDisplayFeedback mDisplayFeedback;        //用于鼠标与控件进行可视化交互
        private IFeature mPanFeature;                     //移动的要素
        private IActiveView pActiveView;

        public Edit(IFeatureLayer editLayer, IMap map,IActiveView pView)
        {
            mCurrentLayer = editLayer;
            this.mMap = map;
            this.pActiveView = pView;
        }

        public bool IsEditing()
        {
            return mIsEditing;
        }

        /// 是否编辑
        public bool HasEdited()
        {
            return mHasEditing;
        }

        /// start editing
        public void StartEditing()
        {
            //获取要素工作空间
            IFeatureClass pFeatureClass = mCurrentLayer.FeatureClass;
            IWorkspace pWorkspace = (pFeatureClass as IDataset).Workspace;
            mWorkspaceEdit = pWorkspace as IWorkspaceEdit;
            if (mWorkspaceEdit == null)
                return;
            //开始编辑
            if (!mWorkspaceEdit.IsBeingEdited())
            {
                mWorkspaceEdit.StartEditing(true);
                mIsEditing = true;
            }
        }

        /// save  editing 
        public void SaveEditing(bool save)
        {
            if (!save)
            {
                mWorkspaceEdit.StopEditing(false);
            }
            else if (save && mHasEditing && mIsEditing)
            {
                mWorkspaceEdit.StopEditing(true);
            }
            mHasEditing = false;
        }

        /// stop editing 
        public void StopEditing(bool save)
        {
            this.SaveEditing(save);
            mIsEditing = false;
        }

        //create point feature or initiate poluline/ploygon feature
        public void CreateMouseDown(double mapX, double mapY)
        {
            //鼠标点击位置
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(mapX, mapY);

            INewLineFeedback pNewLineFeedback;
            INewPolygonFeedback pNewPolygonFeedback;
            //判断编辑状态
            if (mIsEditing)
            {
                //针对线和多边形，判断交互状态，第一次时要初始化，再次点击则直接添加节点
                if (mDisplayFeedback == null)
                {
                    //根据图层类型创建不同要素
                    switch (mCurrentLayer.FeatureClass.ShapeType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            //添加点要素
                            IFeature tempFeature = mCurrentLayer.FeatureClass.CreateFeature();
                            tempFeature.Shape = pPoint;
                            tempFeature.Store();
                            mHasEditing = true;
                            break;
                        case esriGeometryType.esriGeometryPolyline:
                            mDisplayFeedback = new NewLineFeedbackClass();
                            //获取当前屏幕显示
                            mDisplayFeedback.Display = ((IActiveView)this.mMap).ScreenDisplay;
                            pNewLineFeedback = mDisplayFeedback as INewLineFeedback;
                            //开始追踪
                            pNewLineFeedback.Start(pPoint);
                            break;
                        case esriGeometryType.esriGeometryPolygon:
                            mDisplayFeedback = new NewPolygonFeedbackClass();
                            mDisplayFeedback.Display = ((IActiveView)this.mMap).ScreenDisplay;
                            pNewPolygonFeedback = mDisplayFeedback as INewPolygonFeedback;
                            //开始追踪
                            pNewPolygonFeedback.Start(pPoint);
                            break;
                    }

                }
                else //第一次之后的点击则添加节点
                {
                    if (mDisplayFeedback is INewLineFeedback)
                    {
                        pNewLineFeedback = mDisplayFeedback as INewLineFeedback;
                        pNewLineFeedback.AddPoint(pPoint);
                    }
                    else if (mDisplayFeedback is INewPolygonFeedback)
                    {
                        pNewPolygonFeedback = mDisplayFeedback as INewPolygonFeedback;
                        pNewPolygonFeedback.AddPoint(pPoint);
                    }
                }
            }
        }

        //move mouse and feedback function on mapview
        public void pMouseMove(double mapX, double mapY)
        {
            if (mDisplayFeedback == null)
                return;
            //获取鼠标移动点位，并移动至当前点位
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(mapX, mapY);
            mDisplayFeedback.MoveTo(pPoint);
        }

        //double click to create polyline/polygon
        public void CreateDoubleClick(double mapX, double mapY)
        {
            if (mDisplayFeedback == null)
                return;
            IGeometry pGeometry = null;
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(mapX, mapY);

            INewLineFeedback pNewLineFeedback;
            INewPolygonFeedback pNewPolygonFeedback;
            IPointCollection pPointCollection;
            //判断编辑状态
            if (mIsEditing)
            {
                if (mDisplayFeedback is INewLineFeedback)
                {
                    pNewLineFeedback = mDisplayFeedback as INewLineFeedback;
                    //添加点击点
                    pNewLineFeedback.AddPoint(pPoint);
                    //结束Feedback
                    IPolyline pPolyline = pNewLineFeedback.Stop();
                    pPointCollection = pPolyline as IPointCollection;
                    //至少两点时才创建线要素
                    if (pPointCollection.PointCount < 2)
                        MessageBox.Show("至少需要两点才能建立线要素！", "提示");
                    else
                        pGeometry = pPolyline as IGeometry;
                }
                else if (mDisplayFeedback is INewPolygonFeedback)
                {
                    pNewPolygonFeedback = mDisplayFeedback as INewPolygonFeedback;
                    //添加点击点
                    pNewPolygonFeedback.AddPoint(pPoint);
                    //结束Feedback
                    IPolygon pPolygon = pNewPolygonFeedback.Stop();
                    pPointCollection = pPolygon as IPointCollection;
                    //至少三点才能创建线要素
                    if (pPointCollection.PointCount < 3)
                        MessageBox.Show("至少需要两点才能建立线要素！", "提示");
                    else
                        pGeometry = pPolygon as IGeometry;
                }
                mDisplayFeedback.Display = ((IActiveView)this.mMap).ScreenDisplay;
                //不为空时添加
                if (pGeometry != null)
                {
                    IFeature tempFeature = mCurrentLayer.FeatureClass.CreateFeature();
                    tempFeature.Shape = pGeometry;
                    tempFeature.Store(); 
                    mHasEditing = true;
                    //创建完成将DisplayFeedback置为空
                    mDisplayFeedback = null;
                }
            }
        }

        //click down to select the feature
        public void PanMouseDown(double mapX, double mapY)
        {
            //清除地图选择集
            mMap.ClearSelection();
            //获取鼠标点击位置
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(mapX, mapY);

            IActiveView pActiveView = mMap as IActiveView;
            //获取点击到的要素
            mPanFeature = SelectFeature(pPoint);
            if (mPanFeature == null)
                return;
            //获取要素形状
            IGeometry pGeometry = mPanFeature.Shape;

            IMovePointFeedback pMovePointFeedback;
            IMoveLineFeedback pMoveLineFeedback;
            IMovePolygonFeedback pMovePolygonFeedback;
            //根据要素类型定义移动方式
            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    mDisplayFeedback = new MovePointFeedbackClass();
                    //获取屏幕显示
                    mDisplayFeedback.Display = pActiveView.ScreenDisplay;
                    //开始追踪
                    pMovePointFeedback = mDisplayFeedback as IMovePointFeedback;
                    pMovePointFeedback.Start((IPoint)pGeometry, pPoint);
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    mDisplayFeedback = new MoveLineFeedbackClass();
                    mDisplayFeedback.Display = pActiveView.ScreenDisplay;
                    //开始追踪
                    pMoveLineFeedback = mDisplayFeedback as IMoveLineFeedback;
                    pMoveLineFeedback.Start((IPolyline)pGeometry, pPoint);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    mDisplayFeedback = new MovePolygonFeedbackClass();
                    mDisplayFeedback.Display = pActiveView.ScreenDisplay;
                    //开始追踪
                    pMovePolygonFeedback = mDisplayFeedback as IMovePolygonFeedback;
                    pMovePolygonFeedback.Start((IPolygon)pGeometry, pPoint);
                    break;
            }
        }

        public void PanMouseUp(double mapX, double mapY)
        {
            if (mDisplayFeedback == null)
                return;
            //获取点位
            IActiveView pActiveView = mMap as IActiveView;
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(mapX, mapY);

            IMovePointFeedback pMovePointFeedback;
            IMoveLineFeedback pMoveLineFeedback;
            IMovePolygonFeedback pMovePolygonFeedback;
            IGeometry pGeometry;
            //根据移动要素类型选择移动方式  
            if (mDisplayFeedback is IMovePointFeedback)
            {
                pMovePointFeedback = mDisplayFeedback as IMovePointFeedback;
                //结束追踪
                pGeometry = pMovePointFeedback.Stop();
                //更新要素
                UpdateFeature(mPanFeature, pGeometry);
            }
            else if (mDisplayFeedback is IMoveLineFeedback)
            {
                pMoveLineFeedback = mDisplayFeedback as IMoveLineFeedback;
                //结束追踪
                pGeometry = pMoveLineFeedback.Stop();
                //更新要素
                UpdateFeature(mPanFeature, pGeometry);
            }
            else if (mDisplayFeedback is IMovePolygonFeedback)
            {
                pMovePolygonFeedback = mDisplayFeedback as IMovePolygonFeedback;
                pGeometry = pMovePolygonFeedback.Stop();
                int tt = mPanFeature.OID;
                UpdateFeature(mCurrentLayer.FeatureClass.GetFeature(tt), pGeometry);
            }
            mHasEditing = true;
            mDisplayFeedback = null;
            pActiveView.Refresh();
        }

        public IFeature SelectFeature(IPoint pPoint)
        {
            IFeatureClass pFeatureClass;
            //获取图层和要素类，为空时返回         
            pFeatureClass = mCurrentLayer.FeatureClass;
            double length;
            //获取视图范围
            //2个像素大小的屏幕距离转换为地图距离
            length = ConvertPixelToMapUnits(pActiveView, 2);
            ITopologicalOperator pTopoOperator;
            IGeometry pGeoBuffer;
            ISpatialFilter pSpatialFilter;
            //根据缓冲半径生成空间过滤器
            pTopoOperator = pPoint as ITopologicalOperator;
            pGeoBuffer = pTopoOperator.Buffer(length);
            pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeoBuffer;
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
            pFeatureCursor = mCurrentLayer.Search(pQueryFilter, true);
            pFeature = pFeatureCursor.NextFeature();
            return pFeature;
        }

        public void UpdateFeature(IFeature pFeature,IGeometry pGeometry)
        {
            pFeature.Shape = pGeometry;
            pFeature.Store();
        }
        private double ConvertPixelToMapUnits(IActiveView activeView, double pixelUnits)
        {
            double realWorldDiaplayExtent;
            int pixelExtent;
            double sizeOfOnePixel;
            double mapUnits;

            //获取设备中视图显示宽度，即像素个数
            pixelExtent = activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().right - activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().left;
            //获取地图坐标系中地图显示范围
            realWorldDiaplayExtent = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            //每个像素大小代表的实际距离
            sizeOfOnePixel = realWorldDiaplayExtent / pixelExtent;
            //地理距离
            mapUnits = pixelUnits * sizeOfOnePixel;

            return mapUnits;
        }

    }
}
