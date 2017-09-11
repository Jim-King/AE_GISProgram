﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace MyGIS
{
    public partial class Scene_Point_Query : Form
    {
        public Scene_Point_Query()
        {
            InitializeComponent();
        }
        public void refreshView(IHit3DSet pHit3Dset)
        {
            //用tree控件显示查询结果
            mTreeView.BeginUpdate();
            //清空tree控件的内容
            mTreeView.Nodes.Clear();
            IHit3D pHit3D;
            int i;
            //遍历结果集
            for (i = 0; i < pHit3Dset.Hits.Count; i++)
            {
                pHit3D = pHit3Dset.Hits.get_Element(i) as IHit3D;
                if (pHit3D.Owner is ILayer)
                {
                    ILayer pLayer = pHit3D.Owner as ILayer;
                    //将图层的名称和坐标显示在树节点中
                    TreeNode node = mTreeView.Nodes.Add(pLayer.Name);
                    node.Nodes.Add("X=" + pHit3D.Point.X.ToString());
                    node.Nodes.Add("Y=" + pHit3D.Point.Y.ToString());
                    node.Nodes.Add("Z=" + pHit3D.Point.Z.ToString());
                    //将该图层中的所有元素显示在该树节点的子节点
                    if (pHit3D.Object != null)
                    {
                        if (pHit3D.Object is IFeature)
                        {
                            IFeature pFeature = pHit3D.Object as IFeature;
                            int j;
                            //显示Feature中的内容
                            for (j = 0; j < pFeature.Fields.FieldCount; j++)
                            {
                                node.Nodes.Add(pFeature.Fields.get_Field(j).Name + ":" +
                                   pFeature.get_Value(j).ToString());
                            }
                        }
                    }
                }
            }
            mTreeView.EndUpdate();

        }
    }
}
