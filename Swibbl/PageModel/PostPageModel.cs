using Swibbl.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swibbl.Pages
{
    public class PostPageModel : FreshMvvm.FreshBasePageModel
    {
        public override void Init(object initData)
        {
            Post = initData as Post;
        }

        public Post Post { get; set; }
    }
}
