using System;
public static class IntExtentions
{
    public static Nullable<int> ValueOrNull(this Nullable<int> value)
    {
        if(value == null){
            return null;
        }
        
        if(value.HasValue && value.Value > 0){
            return value.Value;
        }

        return null;
    }

}