   M S B A M L   `   `   `  ���� ~  zИнформационная система для больницы, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Qclr-namespace:Информационная_система_для_больницыCИнформационная_система_для_больницы  T    NИнформационная_система_для_больницы.AuthWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     9i+http://schemas.microsoft.com/xaml/behaviors     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     clocalQclr-namespace:Информационная_система_для_больницы     -     �Name$  
authWindow�� � Title$d ^Информационная система для больницы – Авторизация��$	��153��$	��300�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � AllowsTransparency . 
������  � Icon$" /Images/colorfulhospital.png��.����    	pageColor#���    �� CornerRadius$ 10��  	textColor#���  $��2q�._��  � ColumnDefinitions �� $��70 ��� 	 � RowDefinitions	 �� $��30 ��� �� �� .+��� $��0��$��2��.+��� $��Авторизация��$��Bahnschrift)�$��15(�$��Center=�#���  �� $��1��$��0��$��Сотрудник��$��Bahnschrift)�$��10(�$
��Left=�#���  $��	5 10 0 10q��� -   
 ��Name$
 
userPicker��  ComboBoxItemContainerStyle1#���    ComboBoxStyle1#���  $��1��$��1��$��Bahnschrift)�$��10(�$��
0 10 10 10q� �� MaxDropDownHeight$ 90�� �� 
IsEditable . �� $��2��$��0��$��Пароль��$��Bahnschrift)�$��10(�$
��Left=�#���  $��	5 10 0 10q�G� -    G�Name$
 pass��  PasswordBoxStyle#��[ $��2��$��1��$��Bahnschrift)�$��10(�$��
0 10 10 10q��� -     ButtonStyle1#��[ $��3��$��1��$��Right=�$��Bottom=�$��Bahnschrift)�$��10(�$��Вход��$��	0 0 70 10q�$��	15 5 15 5q� �� 	IsDefault . �� -   #���  $��3��$��1��$��Right=�$��Bottom=�$��Bahnschrift)�$��10(�$��Закрыть��$��	0 0 10 10q�$��5q� �� IsCancel . o� $��0��$��1��$	��Top=�$��Right=�
������ .@�m� -     windowMenuButtonsStyle#��[ $��20��$��20��$	��-15q����� $��5��$��5�� 	 cross#���   �� BitmapScalingMode$ HighQuality=�