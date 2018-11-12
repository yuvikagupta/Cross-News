using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;

namespace CrossNews.Droid.Common
{
    public class LineDividerItemDecoration : RecyclerView.ItemDecoration
    {
        private readonly Drawable _divider;

        public LineDividerItemDecoration(Context context)
        {
            _divider = ContextCompat.GetDrawable(context, Resource.Drawable.line_divider);
        }

        public override void OnDrawOver(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            var left = parent.PaddingLeft;
            var right = parent.Width - parent.PaddingRight;

            var childCount = parent.ChildCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = parent.GetChildAt(i);
                var layoutParams = (RecyclerView.LayoutParams) child.LayoutParameters;

                var top = child.Bottom + layoutParams.BottomMargin;
                var bottom = top + _divider.IntrinsicHeight;

                _divider.SetBounds(left, top, right, bottom);
                _divider.Draw(c);
            }
        }
    }
}
