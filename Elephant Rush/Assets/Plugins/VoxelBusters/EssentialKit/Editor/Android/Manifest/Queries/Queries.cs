﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.Editor.Android
{
    public class Queries : Element
    {
        public void Add(Intent element)
        {
            base.Add(element);
        }

        public void Add(Package element)
        {
            base.Add(element);
        }

        protected override string GetName()
        {
            return "queries";
        }
    }
}
