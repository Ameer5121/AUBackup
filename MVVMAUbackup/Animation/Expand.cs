using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MVVMAUbackup.Commands;

namespace MVVMAUbackup.Animation
{
    class Expand
    {
        #region Fields
        private static int ClickLimit;
        private static int ClickLimit2;
        #endregion

        #region Commands
        public ICommand SettingsExpand => new RelayCommand(SettingExpand);
        public ICommand BackupExpand => new RelayCommand(BackUpExpand);
        #endregion

        private void SettingExpand(object Controls)
        {
            var ControlsToAnimate = (object[])Controls;
            // 0 = Panel, 1 = Button
            if (ClickLimit == 0)
            {
                ClickLimit++;
                DoubleAnimation doubleanimation = new DoubleAnimation(319, new TimeSpan(0, 0, 0, 0, 200));
                DoubleAnimation doubleanimation2 = new DoubleAnimation(180.232, new TimeSpan(0, 0, 0, 0, 200));
                ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(444, 30, 0, 125), new TimeSpan(0, 0, 0, 0, 200));
                Storyboard.SetTarget(doubleanimation, (DependencyObject)ControlsToAnimate[0]); // Causes the specified Timeline to target the specified object.
                Storyboard.SetTargetProperty(doubleanimation, new PropertyPath(Border.HeightProperty));
                Storyboard.SetTarget(doubleanimation2, (DependencyObject)ControlsToAnimate[1]);
                Storyboard.SetTargetProperty(doubleanimation2, new PropertyPath("RenderTransform.(RotateTransform.Angle)"));
                Storyboard.SetTarget(thicknessAnimation, (DependencyObject)ControlsToAnimate[0]);
                Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Border.MarginProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleanimation);
                storyboard.Children.Add(doubleanimation2);
                storyboard.Children.Add(thicknessAnimation);
                storyboard.Begin();

            }
            else
            {
                ClickLimit--;
                DoubleAnimation doubleanimation = new DoubleAnimation(24, new TimeSpan(0, 0, 0, 0, 200));
                DoubleAnimation doubleanimation2 = new DoubleAnimation(0, new TimeSpan(0, 0, 0, 0, 200));
                ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(444, 30, 0, 420), new TimeSpan(0, 0, 0, 0, 200));
                Storyboard.SetTarget(doubleanimation, (DependencyObject)ControlsToAnimate[0]);
                Storyboard.SetTargetProperty(doubleanimation, new PropertyPath(Border.HeightProperty));
                Storyboard.SetTarget(doubleanimation2, (DependencyObject)ControlsToAnimate[1]);
                Storyboard.SetTargetProperty(doubleanimation2, new PropertyPath("RenderTransform.(RotateTransform.Angle)"));
                Storyboard.SetTarget(thicknessAnimation, (DependencyObject)ControlsToAnimate[0]);
                Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Border.MarginProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleanimation);
                storyboard.Children.Add(doubleanimation2);
                storyboard.Children.Add(thicknessAnimation);
                storyboard.Begin();
            }


        }

        private void BackUpExpand(object Controls)
        {
            var ControlsToAnimate = (object[])Controls;
            if (ClickLimit2 == 0)
            {
                ClickLimit2++;
                DoubleAnimation doubleanimation = new DoubleAnimation(434, new TimeSpan(0, 0, 0, 0, 200));
                DoubleAnimation doubleanimation2 = new DoubleAnimation(180.459, new TimeSpan(0, 0, 0, 0, 200));
                ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 30, 187, 10), new TimeSpan(0, 0, 0, 0, 200));
                Storyboard.SetTarget(doubleanimation, (DependencyObject)ControlsToAnimate[0]); // Causes the specified Timeline to target the specified object.
                Storyboard.SetTargetProperty(doubleanimation, new PropertyPath(Border.HeightProperty));
                Storyboard.SetTarget(doubleanimation2, (DependencyObject)ControlsToAnimate[1]);
                Storyboard.SetTargetProperty(doubleanimation2, new PropertyPath("RenderTransform.(RotateTransform.Angle)"));
                Storyboard.SetTarget(thicknessAnimation, (DependencyObject)ControlsToAnimate[0]);
                Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Border.MarginProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleanimation);
                storyboard.Children.Add(doubleanimation2);
                storyboard.Children.Add(thicknessAnimation);
                storyboard.Begin();

            }
            else
            {
                ClickLimit2--;
                DoubleAnimation doubleanimation = new DoubleAnimation(24, new TimeSpan(0, 0, 0, 0, 200));
                DoubleAnimation doubleanimation2 = new DoubleAnimation(0, new TimeSpan(0, 0, 0, 0, 200));
                ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 30, 187, 420), new TimeSpan(0, 0, 0, 0, 200));
                Storyboard.SetTarget(doubleanimation, (DependencyObject)ControlsToAnimate[0]);
                Storyboard.SetTargetProperty(doubleanimation, new PropertyPath(Border.HeightProperty));
                Storyboard.SetTarget(doubleanimation2, (DependencyObject)ControlsToAnimate[1]);
                Storyboard.SetTargetProperty(doubleanimation2, new PropertyPath("RenderTransform.(RotateTransform.Angle)"));
                Storyboard.SetTarget(thicknessAnimation, (DependencyObject)ControlsToAnimate[0]);
                Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Border.MarginProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleanimation);
                storyboard.Children.Add(doubleanimation2);
                storyboard.Children.Add(thicknessAnimation);
                storyboard.Begin();
            }


        }
    }
}
