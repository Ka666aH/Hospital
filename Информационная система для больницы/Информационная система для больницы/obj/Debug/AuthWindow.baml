   M S B A M L   `   `   `  ����~  zИнформационная система для больницы, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Qclr-namespace:Информационная_система_для_больницыCИнформационная_система_для_больницы  5   	   T    NИнформационная_система_для_больницы.AuthWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   9i+http://schemas.microsoft.com/xaml/behaviors     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   clocalQclr-namespace:Информационная_система_для_больницы     5   	   -   5          �Name$  
authWindow��5   	    � Title$d ^Информационная система для больницы – Авторизация��5
   	   $	��153��6C   $	��300��6P    � WindowStartupLocation$ CenterScreen=�6\    � WindowStyle$
 None=�6�    � 
ResizeMode$ NoResize=�6�    � AllowsTransparency . 
������  � Icon$" /Images/colorfulhospital.png��5   "   .��5      ��    	pageColor#���   6    �� CornerRadius$ 10��66     	textColor#���  6H   $��2q�6r   ._�5      �  � ColumnDefinitions 5   
   �� 5      $��70 �6   �� 5      	 � RowDefinitions	 5   
   �� 5      $��30 �6   �� 5      �� 5      �� 5      .+�5      �� $��0��6   $��2��6)   .+�5      �� $��Авторизация��6   $��Bahnschrift)�6.   $��15(�6G   $��Center=�6U   #���  6r   5      �� 5      $��1��6   $��0��6!   $��Сотрудник��61   $��Bahnschrift)�6E   $��10(�6^   $
��Left=�6m   #���  6�   $��	5 10 0 10q�6�   �� 5      -   5        
 ��Name$
 
userPicker��5   �     ComboBoxItemContainerStyle1#���  6     ComboBoxStyle1#���  6Z   $��1��6�   $��1��6�   $��Bahnschrift)�6�   $��10(�6�   $��
0 10 10 10q�6�    �� MaxDropDownHeight$ 90��6�    �� 
IsEditable . 6  �� 5       $��2��6   $��0��6!   $��Пароль��61   $��Bahnschrift)�6B   $��10(�6[   $
��Left=�6j   #���  6�   $��	5 10 0 10q�6�   G� 5"      -   5         G�Name$
 pass��5"        PasswordBoxStyle#��[ 6(   $��2��6R   $��1��6_   $��Bahnschrift)�6o   $��10(�6�   $��
0 10 10 10q�6�   5$      �� 5'   
   -   5          ButtonStyle1#��[ 5'      $��3��67   $��1��6D   $��Right=�6T   $��Bottom=�6p   $��Bahnschrift)�6�   $��10(�6�   $��Вход��6�   $��	0 0 70 10q�6�   $��	15 5 15 5q�6�    �� 	IsDefault . 6�   �� 5(      -   5        #���  5(      $��3��6<   $��1��6I   $��Right=�6Y   $��Bottom=�6u   $��Bahnschrift)�6�   $��10(�6�   $��Закрыть��6�   $��	0 0 10 10q�6�   $��5q�6�    �� IsCancel . 6�   o� 5*      $��0��6   $��1��6    $	��Top=�60   $��Right=�6H   
������ .@�5+      m� -   5          windowMenuButtonsStyle#��[ 5+      $��20��6K   $��20��6W   $	��-15q�6b   ��5,      �� 5-      $��5��6   $��5��6'    	 cross#���  61    �� BitmapScalingMode$ HighQuality=�6R   5/      50      51      52      53      