   M S B A M L   `   `   `  ���� ~  zИнформационная система для больницы, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Qclr-namespace:Информационная_система_для_больницыCИнформационная_система_для_больницы  T    NИнформационная_система_для_больницы.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     R NOxyPlot.Wpf, Version=2.2.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0Y UOxyPlot.Wpf.Shared, Version=2.2.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0"oxyhttp://oxyplot.org/wpf   _localQclr-namespace:Информационная_система_для_больницы   -     � Title$I  CИнформационная система для больницы��$	��460��$	��900��$	��460��$	��900�� � WindowStyle$
 None=� � 
ResizeMode$ 	CanResize=� � AllowsTransparency . 
������  � Icon$" /Images/colorfulhospital.png�� � WindowStartupLocation$ CenterScreen=�'  !System.Windows.Shell.WindowChrome   WindowChrome      GlassFrameThickness$ 0,0,0,0q�   CornerRadius$ 0��	   CaptionHeight$	 0Q�.���� -   
 ��Name$
 windowBorder��   backgroundColor#���    �� CornerRadius$ 20��  	textColor#���  ._�� -    �Name$ 
windowGrid�� � ColumnDefinitions �� $	��260 ���  � RowDefinitions �� $��35 ��� $��1* ��� $��3* ��� $	��112 �.+��� $��0��$��0��$��2��$ 10 10 10 10��$��
10 10 0 10q� �� Padding$ 10q�  	menuColor#���  �� $��0��$��2��$��4��$ 10 10 10 10��$��	10 0 0 10q�#���  �� $��0��$��0��  logo#���   �� BitmapScalingMode$ HighQuality=�$��2��$��20q��� -    ��Name$ menuButtons��$��0��$��2��$��Stretch=�$��10 0 0 0q��� $��0��$��3��$	��Top=�$��0 0 0 5q�#���   �� -    ��Name$ generalMenu��$��3��$��Bottom=�$��	10 0 0 10q�.+�	� -    	�Name$ generalMenuView��$��Вид��  menuButtonsStyle#��[ #���  	� -   $ generalMenuChangeUser��$-��'Сменить пользователя��#��[ #���  	� -   $ generalMenuClose��$��
Выйти��#��[ #���  �� -	    ��Name$ currentUser��$��1��$��0��$
��Left=�$��10 5 0 5q�$��Bahnschrift)�$��15(�#���  �� -
   $ 
windowMenu��$��0��$��0��$��3��$��Stretch=�
������ .+�o� $��Stretch=�$��Right=�$��0 5 10 0q�
������ .@�m� -     windowMenuButtonsStyle#��[ $��0 0 20 0q�����   info#���  $ HighQuality=�m� -   #��[ ����  	 minus#���  $ HighQuality=�m� -   #��[ ����  
 square#���  $ HighQuality=�m� -   #��[ ����  		 cross#��� 	 $ HighQuality=��� $��1��$��3��$��1��$��2��$��
10 0 10 10q�$ 5q�$ 10 10 10 10�� 
 	pageColor#��� 
 ._� � -     �Name$ 	mainFrame��  � NavigationUIVisibility$ Hidden=�