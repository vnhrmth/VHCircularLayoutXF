# Circular Layout Xamarin Forms

[![Watch the video](https://i.imgur.com/FmClglm.png)](https://youtu.be/hRpGkBN2I0w)

# Bindable properties

|Bindable Property   |  Default Value | Remark  |
|---|---|---|
| Radius  | 100.0  |  Allows you to change radius based on your layout. |
| Angle  | 30.0  |  Allows you to alter the angle between each element, if you need to overlap elements set a lower angle and lower radius |
| ShouldRotateChild  |  false | Each child is rotated as per their position in the layout.Rotated based on the anchorpoint.  |
|  InitialStartPositionAngle | -180.0 Or 0  | We can use this property to alter the start position of the elements in the circular layout. This property enables you to rotate the elements in circular way.Follows clockwise by default, we can change to anticlockwise by interpolating from negative to positive values.|
|  ShouldStackFirstToLast | true  |  We can stack it top to bottom or bottom. Particularly useful when we want overlapping objects, where we can stack elements in the form of stack or reversed stack. |

# How to use?
Nuget ---> coming soon!!

Meanwhile you can just use copy/paste CircularLayout Class in your project. 
That class will do everything for you. :)

# Usage:
# Generic elements --- Bindable Layout
```
               <local:CircularLayout
                x:Name="testLayout"
                HorizontalOptions="Center"
                VerticalOptions="Start">
                   <BindableLayout.ItemsSource>
                        <x:Array Type="{x:Type x:String}">
                            <x:String>X</x:String>
                            <x:String>Y</x:String>
                            <x:String>Z</x:String>
                            <x:String>D</x:String>
                            <x:String>A</x:String>
                        </x:Array>
                    </BindableLayout.ItemsSource>
                    <BindableLayout.ItemTemplate>
                        <DataTemplate>
                            <Frame
                                Padding="1,1,1,1"
                                BorderColor="Azure"
                                CornerRadius="20"
                                HasShadow="True"
                                HeightRequest="40"
                                WidthRequest="40">
                                <Button
                                    BackgroundColor="LightGreen"
                                    CommandParameter="{Binding .}"
                                    CornerRadius="20"
                                    HeightRequest="40"
                                    WidthRequest="40" />
                            </Frame>
                        </DataTemplate>
                    </BindableLayout.ItemTemplate>
                </local:CircularLayout>
```

# Non Generic elements
 ```           
               <local:CircularLayout
                x:Name="testLayout"
                HorizontalOptions="Center"
                VerticalOptions="Start">
                
               // add child elements using Bindable layout or manually
                <Frame
                    BorderColor="LightGray"
                    HeightRequest="50"
                    WidthRequest="50">
                    <Frame.Background>
                        <LinearGradientBrush StartPoint="0,0" EndPoint="1,0">
                            <GradientStop Offset="0.1" Color="#FFA17F" />
                            <GradientStop Offset="1" Color="#00223E" />
                        </LinearGradientBrush>
                    </Frame.Background>
                    <Frame.Content>
                        <Button Text="Button" TextColor="Cornsilk" />
                    </Frame.Content>
                </Frame>
                ...
                ...
                </local:CircularLayout>
```

Happy Coding!!!
Hope you guys like it :)
