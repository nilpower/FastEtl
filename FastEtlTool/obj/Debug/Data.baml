   M S B A M L   `   `   `  ����  FastEtlTool    FastEtlTool.Data   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $	��600��5   	   $
��1054��6     � 	Resources  5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $ Css/style.xaml?�6&   %    5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��	Bussiness��5         CanUserAddRows . $��15,10,755,14q�6`    J� RenderTransformOrigin$ 0.846,0.541&�6�   ��6v   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 业务名称��6%      CanUserSort .     StyleCellText1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6H   	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 145 6�   
   Binding
 6v   ��  �� Path$
 Name� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   EditingElementStyle 5      �� 5       �� 
TargetType" ��6    .�5      ��  �� Property � �� �� Value$ Center=�6D   �� 5       � ��$ Center=�6B   �� 5       � ��$	 145��66   5      5        5      $ 业务表名��6%    .  # [  6H   $		 120 6�   
 6v   �� $ 	TableName�$ TwoWay=�$ PropertyChanged=� 5      �� 5      " ��6    .�5       ��  � ��$ Center=�6D   �� 5!       � ��$ Center=�6B   �� 5"       � ��$	 120��66   5#      5%      5'        5(   
   -   5        $��BussinessDetails��5(      $��300,10,10,55q�6X    .    FrozenColumnCount$ 2��6�   ��6�   ��  5)        5*      $ 	字段名��6%    .  # [  6F   $		 120 6�   
 6t   �� $ 	FieldName�$ TwoWay=�$ PropertyChanged=� 5+      �� 5,      " ��6    .�5-      ��  � ��$ Center=�6D   �� 5.       � ��$ Center=�6B   �� 5/       � ��$	 120��66   50      52      4  .System.Windows.Controls.DataGridTemplateColumn  54      $		 120 6)   $ 	数据源��57   5      CellTemplate 58      �� 59      .R�5:      �� $��DataSourceBox��6P   -   5        $��Center=�5:   '   $	��110��6D   $��Center=�6e    �� SelectedValuePath$ LinkName��6�    �� DisplayMemberPath$ LinkName��6�   ��6�   �� 6�   5;      5=        5?      $		 120 6)   $ 表名��5B   5    5C      �� 5D      .R�5E      �� $��TabelBox��6P   -   5        $��Center=�5E   '   $	��110��6D   $��Center=�6`   $
 Name��6�   $
 Name��6�   ��6{   �� 6�   5F      5H        5J      $		 120 6)   $ 列名��5M   5    5N      �� 5O      .R�5P      �� $��	ColumnBox��6k   $��Center=�6'   $	��110��6D   $��Center=�6P   $
 Name��6�   $
 Name��6�   ��6|   �� 6�   5Q      5S        5U      $		 120 6)   $ 关联主键��5X   5    5Y      �� 5Z      .R�5[      �� $��KeyBox��6P   $��Center=�6'   $	��110��6D   $��Center=�6^   $
 Name��6�   $
 Name��6�   ��6y   �� 6�   5\      5^        5`      $		 120 6)   $ 排序��5c   5    5d      �� 5e      .R�5f      �� $��
OrderByBox��6P   $��Center=�6'   $	��110��6D   $��Center=�6b   $
 Name��6�   $
 Name��6�   ��6}   �� 6�   5g      5i        5k      $		 120 6)   $ 字典��5n   5    5o      �� 5p      .R�5q      �� $��DicBox��6P   $��Center=�6'   $	��110��6D   $��Center=�6^   $ Id��6�   $
 Name��6�   ��6y   �� 6�   5r      5t        5v      $ 自定义SQL��6%    .  # [  6J   $		 340 6�   
 6x   �� $	 Sql�$ TwoWay=�$ PropertyChanged=� 5w      �� 5x      " ��6    .�5y      ��  � ��$ Center=�6D   �� 5z       � ��$ Center=�6B   �� 5{       � ��$	 340��66   5|      5~      5�      �� 5�   
   -   5        $��保 存��5�      $��28��6   $
��Left=�6+   $��536,525,0,0q�6F   $	��Top=�6[   $��75��6s   6~   �� 5�   
   -   5        $��删 除��5�      $��28��6   $
��Left=�6+   $��651,525,0,0q�6F   $	��Top=�6[   $��75��6s   6~   5�      5�      